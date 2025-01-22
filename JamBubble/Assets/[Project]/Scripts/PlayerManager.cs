using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Rewired;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public Rewired.Player player;
    public Team playerTeam;
    public int playerID;
    public int joystickId;
    private bool isReady = false;

    [SerializeField] private TMP_Text playerText;
    [SerializeField] private GameObject readyText;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    public void SetPlayerStats(int playerId, Team team, int joystickId){
        playerID = playerId;
        this.joystickId = joystickId;
        player = ReInput.players.GetPlayer(playerId);
        player.controllers.AddController(ControllerType.Joystick, joystickId, false);
        playerTeam = team;

        playerText.text = "J" + playerId.ToString("0");
    }

    private void Update(){
        if(!isReady && player.GetButtonDown("Confirm") && !GameManager.Instance.GetAllPlayersReady()){
            isReady = true;
            readyText.SetActive(false); 
            GameManager.Instance.AddReadyPlayer();
        }
        else if(player.GetButtonDown("Confirm") && GameManager.Instance.endGame){
            GameManager.Instance.LeaveGame();
        }
    }
}
