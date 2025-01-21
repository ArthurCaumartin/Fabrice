using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public bool gameStart = false;
    private bool playersTransfer = false;
    private List<PlayerStats> playersStats = new List<PlayerStats>();
    private int playersReady = 0;

    [Header("Params")]
    [SerializeField] private Transform[] leftStartPos;
    [SerializeField] private Transform[] rightStartPos;

    [Header("References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject readyPanel;



    private void Awake(){
        Instance = this;
        StartCoroutine(InitiliazeGame());
    }

    public void GetPlayers(List<PlayerStats> playersStats, GameObject objectToDestroy){
        this.playersStats = playersStats;
        playersTransfer = true;
        Destroy(objectToDestroy);
    }

    private IEnumerator InitiliazeGame(){
        yield return new WaitUntil(() => playersTransfer);
        yield return new WaitUntil(() => InitializePlayers());

        yield return new WaitUntil(() => GetAllPlayersReady());
        DisableReadyUI();
    }

    private bool InitializePlayers(){
        Debug.Log("Initiliaze Players...");
        foreach(PlayerStats playerStats in playersStats){
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(playerStats.playerId, 0, playerStats.playerId), Quaternion.identity);
            Debug.Log(playerStats.playerId + " : " + playerStats.playerTeam);
        }
        return true;
    }

    private bool GetAllPlayersReady(){
        return playersReady == playersStats.Count;
    }

    private void DisableReadyUI(){
        readyPanel.SetActive(false);
    }
}
