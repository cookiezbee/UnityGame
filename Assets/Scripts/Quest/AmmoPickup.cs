using TMPro;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public Vector3 textOffset = new Vector3(0, 2f, 0);

    public GameObject pickupTextPrefab;

    private Transform player;
    private GameObject textInstance;
    private bool isNearPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (pickupTextPrefab != null)
        {
            textInstance = Instantiate(pickupTextPrefab, transform);
            textInstance.transform.localPosition = textOffset;
            textInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isNearPlayer = distance <= pickupRange;

        if (textInstance != null)
        {
            textInstance.SetActive(isNearPlayer);

            if (isNearPlayer)
            {
                textInstance.transform.LookAt(player);
                textInstance.transform.Rotate(0, 180, 0);
            }
        }

        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) PickupAmmo();
    }

    void PickupAmmo()
    {
        WeaponLogic[] allWeapons = player.GetComponentsInChildren<WeaponLogic>(true);

        bool ammoAdded = false;

        foreach (var weapon in allWeapons)
        {
            weapon.AddAmmo(Random.Range(5, 21));
            ammoAdded = true;
        }

        if (ammoAdded) Destroy(gameObject);
    }
}
