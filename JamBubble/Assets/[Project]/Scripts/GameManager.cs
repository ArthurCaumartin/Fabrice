using System.Collections;
using System.Collections.Generic;
using Rewired;
using Rewired.Demos;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public bool onGame = false;
    private bool additionnalTime = false;

    public List<GameObject> playersInGame = new List<GameObject>();
    public List<GameObject> playersLeft = new List<GameObject>();
    public List<GameObject> playersRight = new List<GameObject>();

    private bool playersTransfer = false;
    private List<PlayerStats> playersStats = new List<PlayerStats>();
    private List<PlayerStats> playersReady = new List<PlayerStats>();

    [Header("Params")]
    public float gameTimer = 60;

    public int leftPoint = 0;
    public int rightPoint = 0;

    [SerializeField] private Transform[] leftStartPos;
    [SerializeField] private Transform[] rightStartPos;

    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject countdownText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text leftPointText;
    [SerializeField] private TMP_Text rightPointText;
    [SerializeField] CinemachineTargetGroup targetCam;


    private void Awake(){
        Instance = this;
        FindAnyObjectByType<PressToJoinManager>().GetComponent<PressToJoinManager>().SetupGameManager();
        StartCoroutine(InitiliazeGame());
    }

    public void GetPlayers(List<PlayerStats> playersStats){
        this.playersStats = playersStats;
        playersTransfer = true;
    }
    
    void Update(){
        if(onGame){
            gameTimer -= Time.deltaTime;
            if(gameTimer <= 0){
                gameTimer = 0;
                EndGame();
            }

            SetTimerText();
            
        }
    }

    private void SetTimerText(){
        timerText.text = Mathf.Floor(gameTimer/60).ToString("00") + ":" + Mathf.RoundToInt(gameTimer%60).ToString("00");
    }

    private IEnumerator InitiliazeGame(){
        SetTimerText();

        yield return new WaitUntil(() => playersTransfer);
        yield return new WaitUntil(() => InitializePlayers());
        SetPlayerMovement(false);
        yield return new WaitUntil(() => PlaceAllPlayers());

        // A REMETTRE !!
        //yield return new WaitUntil(() => GetAllPlayersReady());
        DisableReadyUI();

        yield return new WaitForSeconds(1f);
        StartGame();
    }

    private bool InitializePlayers(){
        foreach(PlayerStats playerStats in playersStats){
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(playerStats.playerId, 1.2f, playerStats.playerId), Quaternion.identity);
            newPlayer.GetComponent<PlayerManager>().SetPlayerStats(playerStats.playerId, playerStats.playerTeam, playerStats.joystickId);

            targetCam.AddMember(newPlayer.transform, .25f, 0f);

            if(playerStats.playerTeam == Team.Left) playersLeft.Add(newPlayer);
            else playersRight.Add(newPlayer);

            playersInGame.Add(newPlayer);
        }
        return true;
    }

    private bool PlaceAllPlayers(){
        
        for(int i = 0; i < playersLeft.Count; i++){
            playersLeft[i].transform.position = leftStartPos[i].position;
        }
        for(int i = 0; i < playersRight.Count; i++){
            playersRight[i].transform.position = rightStartPos[i].position;
        }      
        return true;
    }

    private bool GetAllPlayersReady(){
        return playersReady.Count == playersInGame.Count;
    }

    private void DisableReadyUI(){
        readyPanel.SetActive(false);
    }

    private void StartGame(){
        StartCoroutine(StartPoint());
    }

    private void SetPlayerMovement(bool active){
        foreach(GameObject player in playersInGame){
            player.GetComponent<PlayerControler>().EnableControler(active, true);
        }        
    }

    public void AddPoint(Team teamPoint){
        leftPoint += teamPoint == Team.Left ? 1 : 0;
        rightPoint += teamPoint == Team.Right ? 1 : 0;

        leftPointText.text = leftPoint.ToString("0");
        rightPointText.text = leftPoint.ToString("0");

        StartCoroutine(EndPoint());
    }

    public IEnumerator EndPoint(){
        onGame = false;
        SetPlayerMovement(false);

        yield return new WaitForSeconds(2f);
        StartCoroutine(StartPoint());
    }

    public IEnumerator StartPoint(){
        SetTimerText();
        yield return new WaitUntil(() => PlaceAllPlayers());
        yield return new WaitForSeconds(1f);
        StartCoroutine(Coutdown(3));
        yield return new WaitForSeconds(3f);
        SetPlayerMovement(true);
        onGame = true;
    }

    private IEnumerator Coutdown(int coutdown){
        for(int i = coutdown; i >= 0; i--){
            countdownText.SetActive(i > 0 ? true : false);
            countdownText.GetComponent<TMP_Text>().text = i.ToString("0");
            yield return new WaitForSeconds(1);            
        }
    }

    private void EndGame(){
        onGame = false;
        SetPlayerMovement(false);

        if(leftPoint != rightPoint || additionnalTime){
            // FINIR LA GAME
            
        }
        else{
            // TEMPS ADDIITIONNEL
            gameTimer = 60;
            additionnalTime = true;
            
            StartCoroutine(StartPoint());
        }
    }
}
