using System.Collections;
using TMPro;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public enum WeaponType { Gun, Melee }

    [SerializeField] WeaponType type = WeaponType.Gun;

    [SerializeField] int maxAmmo = 15;
    [SerializeField] int currentAmmo;

    TextMeshProUGUI ammoText;

    [SerializeField] Transform firingPoint;
    [SerializeField] float range = 7f;
    [SerializeField] float impulse = 10f;
    [SerializeField] int weaponDamage = 3;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] BulletProjectile bulletPrefab;

    [SerializeField] float meleeDelay = 0.5f;

    private bool isAttacking = false;

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

    public void shot()
    {
        if (DialogueController.IsDialogueActive) return;

        if (isAttacking) return;

        if (type == WeaponType.Gun)
        {
            if (currentAmmo > 0)
            {
                if (muzzleFlash != null) muzzleFlash.Play();

                if (bulletPrefab != null)
                {
                    BulletProjectile newBullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
                    newBullet.Init(weaponDamage, impulse);
                }
                
                currentAmmo--;
                UpdateAmmoUI();
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

                EnemyAnimations zombieAnim = hitCollider.GetComponentInParent<EnemyAnimations>();
                if (zombieAnim != null) zombieAnim.PlayHit();

                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null) ammoText.text = currentAmmo + " / " + maxAmmo;
    }
}