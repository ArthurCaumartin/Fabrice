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

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    public void SetPlayerStats(int playerId, Team team){
        player = ReInput.players.GetPlayer(playerId);
        playerTeam = team;
    }
}
