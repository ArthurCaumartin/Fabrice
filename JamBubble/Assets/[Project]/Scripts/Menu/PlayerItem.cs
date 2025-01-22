using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using DG.Tweening;

public class PlayerItem : MonoBehaviour
{
    public Rewired.Player player;
    public int joystickId;
    public int playerId;

    public PressToJoinManager pressToJoinManager;
    public Team actualTeam;

    private float timeBeforeCanChangeTeam;
    private bool canChangeTeam = true;

    public GameObject leftArrow;
    public GameObject rightArrow;

    [SerializeField] private TMP_Text playerIdText;

    public void GetPlayerId(int playerId){
        player = ReInput.players.GetPlayer(playerId);
        this.playerId = playerId;
        playerIdText.text = playerId.ToString("0");
    }

    public void GetJoystickId(int joystickId){
        this.joystickId = joystickId;
    }

    void Update(){
        PlayerInput();

        if(!canChangeTeam && timeBeforeCanChangeTeam < .25f){
            timeBeforeCanChangeTeam += Time.deltaTime;  
            if(timeBeforeCanChangeTeam >= .25f){
                timeBeforeCanChangeTeam = .25f;
                canChangeTeam = true;
            } 
        }
    }

    public void HasChangeTeam(Team newTeam){
        timeBeforeCanChangeTeam = 0;
        canChangeTeam = false;

        leftArrow.SetActive(newTeam == Team.Left ? false : true);
        rightArrow.SetActive(newTeam == Team.Right ? false : true);
    }

    private void PlayerInput(){
        if(player.GetButtonDown("Back")){
            pressToJoinManager.RemovePlayer(player.id, this.gameObject, joystickId);
        }
        if(player.GetButtonDown("Start")){
            pressToJoinManager.TryToStart();
        }

        if(!canChangeTeam) return;
        DOTween.Kill(2, true);

        if(player.GetAxisRaw("Horizontal") >= .5f){
            transform.DOPunchRotation(new Vector3(0,0,1) * 10f, .25f, 20, 1).SetId(2);
            if(actualTeam == Team.None){
                pressToJoinManager.ChangeTeam(Team.Right, this.gameObject, player.id);
            }
            else if(actualTeam == Team.Left){
                pressToJoinManager.ChangeTeam(Team.None, this.gameObject, player.id);
            }
        }
        else if(player.GetAxisRaw("Horizontal") <= -.5f){
            transform.DOPunchRotation(new Vector3(0,0,-1) * 10f, .25f, 20, 1).SetId(2);
            if(actualTeam == Team.None){
                pressToJoinManager.ChangeTeam(Team.Left, this.gameObject, player.id);
            }
            else if(actualTeam == Team.Right){
                pressToJoinManager.ChangeTeam(Team.None, this.gameObject, player.id);
            }
        }
    }
}
