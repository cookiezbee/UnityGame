using UnityEngine;

public class EventParameters
{
    public Vector3 playerPosition;
    public RaycastHit hit;
    public float impulse;

    public EventParameters(Vector3 playerPosition, RaycastHit hit, float impulse)
    {
        this.playerPosition = playerPosition;
        this.hit = hit;
        this.impulse = impulse;
    }
}