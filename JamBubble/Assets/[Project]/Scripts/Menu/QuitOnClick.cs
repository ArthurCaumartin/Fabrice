using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class QuitOnClick : MonoBehaviour
{
    void Start(){
        AudioManager.Instance?.StopMusic();
        AudioManager.Instance?.PlayMusic("credit");
    }

    private void Update() {
        if(!ReInput.isReady) return;
        AssignJoysticksToPlayers();

    }

    private void AssignJoysticksToPlayers() {
        IList<Joystick> joysticks = ReInput.controllers.Joysticks;
        for(int i = 0; i < joysticks.Count; i++) {

            Joystick joystick = joysticks[i];
            if(joystick.GetAnyButtonDown()) {
                TransitionManager.Instance.TransitionToScene("Main Menu");
            }
            
        }

    }
}
