using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public Team teamSide;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Ball" && GameManager.Instance.onGame){
            // PUT POINT
            GameManager.Instance.AddPoint(teamSide == Team.Left ? Team.Right : Team.Left);
        }
    }
}
