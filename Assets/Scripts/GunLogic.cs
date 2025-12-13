using UnityEngine;

public class GunLogic : MonoBehaviour
{
    [SerializeField] Transform firingPoint;
    [SerializeField] float range = 100f;
    [SerializeField] float impulse = 10f;

    [SerializeField] ParticleSystem muzzleFlash;

    public void shot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();

        Ray ray = new Ray(firingPoint.position, firingPoint.forward);
        RaycastHit hit;

        Debug.DrawRay(firingPoint.position, firingPoint.forward * range, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, range))
        {
            TargetHitScript target = hit.transform.GetComponent<TargetHitScript>();
            if (target != null)
            {
                target.targetHit(new EventParameters(transform.position, hit, impulse));
            }
        }
    }
}