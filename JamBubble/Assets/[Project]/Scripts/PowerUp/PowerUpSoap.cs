using UnityEngine;

public class PowerUpSoap : PowerUp
{
    [Header("Soap : ")]
    [SerializeField] private float _sizeToAdd = .2f;

    public override void OnGrab(GameObject playerRef)
    {
        playerRef.GetComponent<BubbleControler>().UpdateSize(_sizeToAdd);
        base.OnGrab(playerRef);
    }
}