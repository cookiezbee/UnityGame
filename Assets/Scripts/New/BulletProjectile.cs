using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float lifeTime = 1f; 

    private int damage;
    private float impulse;

    public void Init(int dmg, float imp)
    {
        damage = dmg;
        impulse = imp;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        TargetHitScript target = other.GetComponent<TargetHitScript>();

        if (target != null)
        {
            RaycastHit hitInfo = new RaycastHit();
            hitInfo.point = transform.position;
            hitInfo.normal = -transform.forward;

            target.targetHit(new EventParameters(transform.position, hitInfo, impulse, damage));
        }

        Destroy(gameObject);
    }
}