using UnityEngine;

public class EventParameters
{
    public Vector3 playerPosition;
    public RaycastHit hit;
    public float impulse;
    public int damage;

    public EventParameters(Vector3 playerPosition, RaycastHit hit, float impulse, int damage)
    {
        this.playerPosition = playerPosition;
        this.hit = hit;
        this.impulse = impulse;
        this.damage = damage;
    }
}