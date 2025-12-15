using UnityEngine;
using System.Collections;

public class WeaponLogic : MonoBehaviour
{
    public enum WeaponType { Gun, Melee }

    [SerializeField] WeaponType type = WeaponType.Gun;

    [SerializeField] Transform firingPoint;
    [SerializeField] float range = 7f;
    [SerializeField] float impulse = 10f;
    [SerializeField] int weaponDamage = 3;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] BulletProjectile bulletPrefab;

    [SerializeField] float meleeDelay = 0.5f;

    private bool isAttacking = false;

<<<<<<< Updated upstream:Assets/Scripts/WeaponLogic.cs
=======
    void Start()
    {
        if (type == WeaponType.Gun)
        {
            if (ammoText == null)
            {
                GameObject foundObject = GameObject.Find("AmmoText");
                if (foundObject != null) ammoText = foundObject.GetComponent<TextMeshProUGUI>();
            }

            currentAmmo = maxAmmo;
            UpdateAmmoUI();
        }
    }

    public void AddAmmo(int amount)
    {
        if (type != WeaponType.Gun) return;

        currentAmmo += amount;
        maxAmmo += amount;

        UpdateAmmoUI();
    }

>>>>>>> Stashed changes:Assets/Scripts/New/WeaponLogic.cs
    public void shot()
    {
        if (DialogueController.IsDialogueActive) return;

        if (isAttacking) return;

        if (type == WeaponType.Gun)
        {
            if (muzzleFlash != null) muzzleFlash.Play();

            if (bulletPrefab != null)
            {
                BulletProjectile newBullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
                newBullet.Init(weaponDamage, impulse);
            }
        }
        else
        {
            StartCoroutine(MeleeStrike());
        }
    }

    IEnumerator MeleeStrike()
    {
        isAttacking = true;

        yield return new WaitForSeconds(meleeDelay);

        Vector3 attackPos = firingPoint.position + (firingPoint.forward * 1.5f);

        Collider[] hitColliders = Physics.OverlapSphere(attackPos, range);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) continue;

            TargetHitScript target = hitCollider.GetComponentInParent<TargetHitScript>();

            if (target != null)
            {
                RaycastHit fakeHit = new RaycastHit();
                fakeHit.point = hitCollider.transform.position;
                fakeHit.normal = (transform.position - hitCollider.transform.position).normalized;

                target.targetHit(new EventParameters(transform.position, fakeHit, impulse, weaponDamage));

                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}