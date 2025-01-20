using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PressToJoinManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject playerItemPrefab;
    [SerializeField] private Transform selectorParent;
    [SerializeField] private Transform leftSelectorParent;
    [SerializeField] private Transform rightSelectorParent;
    [SerializeField] private GameObject canStartPanel;

    [Header("Stats")]
    public List<PlayerItem> playerList = new List<PlayerItem>();
    private int leftTeamCount = 0;
    private int rightTeamCount = 0;

    private bool canStart = false;

    private void Update() {
        if(!ReInput.isReady) return;
        AssignJoysticksToPlayers();
    }

    private void AssignJoysticksToPlayers() {

        // Check all joysticks for a button press and assign it tp
        // the first Player foudn without a joystick
        IList<Joystick> joysticks = ReInput.controllers.Joysticks;
        for(int i = 0; i < joysticks.Count; i++) {

            Joystick joystick = joysticks[i];
            if(ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue; // joystick is already assigned to a Player

            // Chec if a button was pressed on the joystick
            if(joystick.GetAnyButtonDown()) {

                // Find the next Player without a Joystick
                Player player = FindPlayerWithoutJoystick();
                if(player == null) return; // no free joysticks

                // Assign the joystick to this Player
                player.controllers.AddController(joystick, false);
                Debug.Log("Player find id : " + player.id);

                AddPlayerItem(player.id);
            }
        }

        if(CanStart() != canStart){
            canStart = CanStart();
            canStartPanel.SetActive(canStart);
        } 

    }

    public bool CanStart(){
        return leftTeamCount > 0 && leftTeamCount == rightTeamCount;
    }

    // Searches all Players to find the next Player without a Joystick assigned
    private Player FindPlayerWithoutJoystick() {
        IList<Player> players = ReInput.players.Players;
        for(int i = 0; i < players.Count; i++) {
            if(players[i].controllers.joystickCount > 0) continue;
            return players[i];
        }
        return null;
    }
    
    private void AddPlayerItem(int playerId){
        GameObject playerItem = Instantiate(playerItemPrefab, Vector3.zero, Quaternion.identity, selectorParent.GetChild(playerId));
        playerItem.transform.localPosition = Vector3.zero;

        playerItem.GetComponent<PlayerItem>().pressToJoinManager = this;
        playerItem.GetComponent<PlayerItem>().GetPlayerId(playerId);

        playerList.Add(playerItem.GetComponent<PlayerItem>());
    }
    public void RemovePlayer(int playerId, GameObject playerToDestroy){
        Debug.Log("Remove player : " + playerId);
        
        Team actualTeam = playerToDestroy.GetComponent<PlayerItem>().actualTeam;

        leftTeamCount -= actualTeam == Team.Left ? 1 : 0;
        rightTeamCount -= actualTeam == Team.Right ? 1 : 0;
        
        playerList.Remove(playerToDestroy.GetComponent<PlayerItem>());

        Destroy(playerToDestroy);
        Player player = ReInput.players.GetPlayer(playerId);
        player.controllers.RemoveController(ControllerType.Joystick, 0);

        
    }

    public void ChangeTeam(Team newTeam, GameObject playerObject, int playerId){
        Team actualTeam = playerObject.GetComponent<PlayerItem>().actualTeam;

        leftTeamCount -= actualTeam == Team.Left ? 1 : 0;
        rightTeamCount -= actualTeam == Team.Right ? 1 : 0;

        leftTeamCount += newTeam == Team.Left ? 1 : 0;
        rightTeamCount += newTeam == Team.Right ? 1 : 0;

        playerObject.GetComponent<PlayerItem>().actualTeam = newTeam;
        playerObject.GetComponent<PlayerItem>().HasChangeTeam(newTeam);

        if(newTeam != Team.None){
            playerObject.transform.SetParent(newTeam == Team.Left ? leftSelectorParent.GetChild(playerId) : rightSelectorParent.GetChild(playerId));
        }
        else {
            playerObject.transform.SetParent(selectorParent.GetChild(playerId));
        }

        playerObject.transform.localPosition = Vector3.zero;


    }
}

