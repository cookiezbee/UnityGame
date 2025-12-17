using UnityEngine;
using TMPro;

public class Gun : Weapon
{
    public int currentAmmo;

    public Transform firingPoint;
    public float impulse = 10f;
    [SerializeField] public Camera playerCamera;

    public BulletProjectile bulletPrefab;
    public ParticleSystem muzzleFlash;

    private TextMeshProUGUI ammoText;

    public AudioSource gunAudioSource;
    public AudioClip shootSound;
    public AudioClip emptyClickSound;

    private void Start()
    {
        currentAmmo = 15;

        if (playerCamera == null) playerCamera = Camera.main;

        if (gunAudioSource == null) gunAudioSource = GetComponent<AudioSource>();

        GameObject foundObject = GameObject.Find("AmmoText");
        if (foundObject != null) ammoText = foundObject.GetComponent<TextMeshProUGUI>();

        UpdateAmmoUI();
    }

    protected override void PerformAttack() { }

    public override void OnAnimationEventTriggered()
    {
        if (currentAmmo > 0)
        {
            if (gunAudioSource != null && shootSound != null) gunAudioSource.PlayOneShot(shootSound, 0.6f);

            if (muzzleFlash != null) muzzleFlash.Play();

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit)) targetPoint = hit.point;
            else targetPoint = ray.GetPoint(100);

            Vector3 direction = targetPoint - firingPoint.position;

            Quaternion rotationToTarget = Quaternion.LookRotation(direction);

            if (bulletPrefab != null)
            {
                BulletProjectile newBullet = Instantiate(bulletPrefab, firingPoint.position, rotationToTarget);
                newBullet.Init(damage, impulse);
            }

            currentAmmo--;
            UpdateAmmoUI();
        }
        else
        {
            if (gunAudioSource != null && emptyClickSound != null) gunAudioSource.PlayOneShot(emptyClickSound);
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null) ammoText.text = "Патронов: " + currentAmmo;
    }

    private void OnEnable()
    {
        UpdateAmmoUI();
    }
}
