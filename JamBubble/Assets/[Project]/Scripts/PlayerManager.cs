using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    public Rewired.Player player;
    public Team playerTeam;
    public int playerID;
    public int joystickId;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    public void SetPlayerStats(int playerId, Team team, int joystickId){
        playerID = playerId;
        player = ReInput.players.GetPlayer(playerId);
        player.controllers.AddController(ControllerType.Joystick, joystickId, false);
        playerTeam = team;
    }
}
