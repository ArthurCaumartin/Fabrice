using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private bool canJoin = true;


    public void SetupGameManager(){
        List<PlayerStats> playersStats = new List<PlayerStats>();
        foreach(PlayerItem playerItem in playerList){
            playersStats.Add(new PlayerStats(){playerId = playerItem.playerId, playerTeam = playerItem.actualTeam, joystickId = playerItem.joystickId});
        }
        GameManager.Instance.GetPlayers(playersStats);
        Destroy(this.gameObject,.5f);
    }

    private void Update() {
        if(!ReInput.isReady || !canJoin) return;
        AssignJoysticksToPlayers();

    }

    private void AssignJoysticksToPlayers() {
        IList<Joystick> joysticks = ReInput.controllers.Joysticks;
        for(int i = 0; i < joysticks.Count; i++) {

            Joystick joystick = joysticks[i];
            if(ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue; 
            if(joystick.GetButtonDown(1) && playerList.Count == 0){
                SceneManager.LoadScene("Main Menu");
            }
            else if(joystick.GetAnyButtonDown()) {

                Player player = FindPlayerWithoutJoystick();
                if(player == null) return;

                joystick.SetVibration(0, .5f, .5f);
                player.controllers.AddController(joystick, false);

                AddPlayerItem(player.id, joystick);
            }
            
        }

        if(CanStart() != canStart){
            canStart = CanStart();
            canStartPanel.SetActive(canStart);
        } 

    }

    private Player FindPlayerWithoutJoystick() {
        IList<Player> players = ReInput.players.Players;
        for(int i = 0; i < players.Count; i++) {
            if(players[i].controllers.joystickCount > 0) continue;
            return players[i];
        }
        return null;
    }

    public bool CanStart(){
        return leftTeamCount > 0 && leftTeamCount == rightTeamCount;
    }

    private void AddPlayerItem(int playerId, Joystick joystick){
        GameObject playerItem = Instantiate(playerItemPrefab, Vector3.zero, Quaternion.identity, selectorParent.GetChild(playerId));
        playerItem.transform.localPosition = Vector3.zero;

        playerItem.GetComponent<PlayerItem>().pressToJoinManager = this;
        playerItem.GetComponent<PlayerItem>().GetPlayerId(playerId);
        playerItem.GetComponent<PlayerItem>().GetJoystickId(joystick.id);

        playerList.Add(playerItem.GetComponent<PlayerItem>());
    }

    public void RemovePlayer(int playerId, GameObject playerToDestroy, int joystickId){
        Team actualTeam = playerToDestroy.GetComponent<PlayerItem>().actualTeam;

        leftTeamCount -= actualTeam == Team.Left ? 1 : 0;
        rightTeamCount -= actualTeam == Team.Right ? 1 : 0;
        
        playerList.Remove(playerToDestroy.GetComponent<PlayerItem>());

        Destroy(playerToDestroy);
        Player player = ReInput.players.GetPlayer(playerId);
        player.controllers.RemoveController(ControllerType.Joystick, joystickId);
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

    public void TryToStart(){
        if(canStart){
            canJoin = false;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene("Game");
        }
    }
}

