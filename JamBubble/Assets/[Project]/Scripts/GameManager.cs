using System.Collections;
using System.Collections.Generic;
using Rewired;
using Rewired.Demos;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public bool onGame = false;
    public bool endGame = false;
    private bool additionnalTime = false;

    public List<GameObject> playersInGame = new List<GameObject>();
    public List<GameObject> playersLeft = new List<GameObject>();
    public List<GameObject> playersRight = new List<GameObject>();

    private bool playersTransfer = false;
    private List<PlayerStats> playersStats = new List<PlayerStats>();
    private int playersReady = 0;

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
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text leftPointText;
    [SerializeField] private TMP_Text rightPointText;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private TMP_Text leftPointEndText;
    [SerializeField] private TMP_Text rightPointEndText;
    [SerializeField] private GameObject additionalTimeObject;
    [SerializeField] CinemachineTargetGroup targetCam;
    [SerializeField] private GameObject gameCamera;


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
        yield return new WaitUntil(() => PlaceAllPlayers());
        SetPlayerMovement(false);
        
        yield return new WaitUntil(() => GetAllPlayersReady());
        
        readyPanel.transform.DOLocalMoveY(-735f,.25f).SetEase(Ease.OutBounce);
        scoreboard.SetActive(true);
        scoreboard.transform.DOLocalMoveY(540,.5f).From(640).SetEase(Ease.OutBounce);

        yield return new WaitForSeconds(.25f);
        DisableReadyUI();
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
            Physics.SyncTransforms();
        }
        for(int i = 0; i < playersRight.Count; i++){
            playersRight[i].transform.position = rightStartPos[i].position;
            Physics.SyncTransforms();
        }      
        return true;
    }

    public void AddReadyPlayer(){
        playersReady++;
    }

    public bool GetAllPlayersReady(){
        return playersReady == playersInGame.Count;
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
        rightPointText.text = rightPoint.ToString("0");

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
        //additionalTimeObject.SetActive(false);
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
            endGame = true;
            DisplayEndUI();
        }
        else{
            gameTimer = 60;
            additionnalTime = true;
            additionalTimeObject.SetActive(true);
            additionalTimeObject.transform.DOLocalMoveY(425, 1f).From(615).SetEase(Ease.OutElastic);
            
            StartCoroutine(StartPoint());
        }
    }

    private void DisplayEndUI(){
        endCanvas.SetActive(true);
        endCanvas.transform.DOLocalMoveY(0f, 1f).From(1100).SetEase(Ease.OutElastic);
        leftPointEndText.text = leftPoint.ToString("0");
        rightPointEndText.text = rightPoint.ToString("0");
        if(leftPoint > rightPoint){
            leftPointEndText.transform.DOScale(1.1f, 0.5f).From(1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else if(rightPoint > leftPoint){
            rightPointText.transform.DOScale(1.1f, 0.5f).From(1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
    }

    public void LeaveGame(){
        SceneManager.LoadScene("Main Menu");
    }
}
