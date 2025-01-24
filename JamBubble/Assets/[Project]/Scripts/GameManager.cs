using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using TMPro;
using Cinemachine;
using DG.Tweening;
using System.Linq;

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

    public List<GameObject> objectsToDestroy = new List<GameObject>();

    [Header("Params")]
    [SerializeField] private Transform ball;
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
    [SerializeField] private GameObject goalCanvas;
    [SerializeField] private TMP_Text teamNameText;
    [SerializeField] CinemachineTargetGroup targetCam;
    [SerializeField] private CameraShake camShake;
    [SerializeField] private GameObject arbitre;



    private void Awake(){
        Instance = this;
        FindAnyObjectByType<PressToJoinManager>().GetComponent<PressToJoinManager>().SetupGameManager();
        StartCoroutine(InitiliazeGame());

        ReInput.ControllerConnectedEvent += OnControllerConnected;
    }

    void OnDestroy() {
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args) {
        GameObject playerItemToConnect = playersInGame.Where(x => x.GetComponent<PlayerManager>().joystickId == args.controllerId).SingleOrDefault();
        playerItemToConnect.GetComponent<PlayerManager>().ReconnectController();
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
        Vector3 targetPostition = new Vector3( ball.position.x, 
                                       arbitre.transform.position.y, 
                                       ball.position.z ) ;
        arbitre.transform.LookAt( targetPostition ) ;
    }

    private void SetTimerText(){
        timerText.text = Mathf.Floor(gameTimer/60).ToString("00") + ":" + Mathf.RoundToInt(gameTimer%60).ToString("00");
    }

    private IEnumerator InitiliazeGame(){
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlayMusic("game");
        SetTimerText();

        yield return new WaitUntil(() => playersTransfer);
        yield return new WaitUntil(() => InitializePlayers());
        yield return new WaitUntil(() => PlaceAllPlayers());
        SetPlayerMovement(false);
        
        yield return new WaitUntil(() => GetAllPlayersReady());
        
        readyPanel.GetComponent<CanvasGroup>().DOFade(0f,.5f);
        scoreboard.SetActive(true);
        scoreboard.transform.DOLocalMoveY(525,.5f).From(640).SetEase(Ease.OutBounce);

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
            playersLeft[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            playersLeft[i].transform.position = leftStartPos[i].position;
            Physics.SyncTransforms();
        }
        for(int i = 0; i < playersRight.Count; i++){
            playersRight[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            playersRight[i].transform.position = rightStartPos[i].position;
            Physics.SyncTransforms();
        }      

        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.position = new Vector3(0,3,0);
        Physics.SyncTransforms();

        foreach(GameObject objectToDes in objectsToDestroy){
            if(objectToDes) Destroy(objectToDes);
        }
        objectsToDestroy.Clear();
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

    public void Shake(float time, float force){
        camShake.shakeDuration = time;
        camShake.shakeAmount = force;
    }

    public void AddPoint(Team teamPoint){
        Shake(1.5f, 1f);
        AudioManager.Instance.PlaySFX("goal", .5f);
        arbitre.transform.GetChild(0).DOLocalMoveY(.3f, .5f).From(0.125f).SetEase(Ease.OutBounce).SetInverted();
        arbitre.transform.GetChild(0).DOLocalRotate(new Vector3(-0.25f,0,0), .5f).From(Vector3.zero).SetEase(Ease.OutBounce).SetInverted();

        ControllerVibrationEveryone(.5f,1.5f);
        

        teamNameText.text = teamPoint == Team.Left ? "Shark" : "Fish";
        teamNameText.color = teamPoint == Team.Left ? Color.cyan : Color.red;

        leftPoint += teamPoint == Team.Left ? 1 : 0;
        rightPoint += teamPoint == Team.Right ? 1 : 0;

        leftPointText.text = leftPoint.ToString("0");
        rightPointText.text = rightPoint.ToString("0");

        StartCoroutine(EndPoint());
    }

    public void ControllerVibrationEveryone(float force, float duration){
        foreach(GameObject _player in playersInGame){
            ControllerVibration(_player.GetComponent<PlayerManager>().joystickId, force, duration);
        }
    }

    public void ControllerVibration(int joystickId, float force, float duration){
        Joystick joystick = ReInput.controllers.GetJoystick(joystickId);
        if(joystick == null || !joystick.supportsVibration) return;
        joystick.StopVibration();
        if(joystick.vibrationMotorCount > 0) joystick.SetVibration(0, force, duration);
    }

    public IEnumerator EndPoint(){
        yield return new WaitForSeconds(.25f);
        goalCanvas.SetActive(true);
        goalCanvas.transform.DOLocalMoveY(0f, 1f).From(1100).SetEase(Ease.OutElastic);
        onGame = false;
        SetPlayerMovement(false);

        yield return new WaitForSeconds(4f);

        SetPlayerMovement(false);
        goalCanvas.SetActive(false);
        if(!additionnalTime) StartCoroutine(StartPoint());
        else {
            EndGame();
        }
    }

    public IEnumerator StartPoint(){
        SetTimerText();
        yield return new WaitUntil(() => PlaceAllPlayers());
        yield return new WaitForSeconds(1f);
        StartCoroutine(Coutdown(3));
        yield return new WaitForSeconds(3f);
        AudioManager.Instance.PlaySFX("start", .5f);
        SetPlayerMovement(true);
        onGame = true;
    }

    private IEnumerator Coutdown(int coutdown){
        for(int i = coutdown; i >= 0; i--){
            ControllerVibrationEveryone(.25f,.5f);
            countdownText.SetActive(i > 0 ? true : false);
            countdownText.GetComponent<TMP_Text>().text = i.ToString("0");
            yield return new WaitForSeconds(1);            
        }
    }

    private void EndGame(){
        onGame = false;
        SetPlayerMovement(false);
        

        if(leftPoint != rightPoint || additionnalTime){
            AudioManager.Instance.PlaySFX("finish", .5f);
            endGame = true;
            DisplayEndUI();
            ControllerVibrationEveryone(.35f,1.5f);
        }
        else{
            AudioManager.Instance.PlaySFX("additional_time", .5f);
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
        AudioManager.Instance.PlaySFX("ui_cancel"); 
        TransitionManager.Instance.TransitionToScene("Main Menu");
        DestroyImmediate(this);
    }
}
