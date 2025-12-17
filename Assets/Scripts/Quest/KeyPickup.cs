using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public Vector3 textOffset = new Vector3(0, 1.5f, 0);

    public GameObject pickupTextPrefab;

    private Transform player;
    private GameObject textInstance;
    private bool isNearPlayer = false;

    public AudioClip keyPickupSound;

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

        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) PickupKey();
    }

    void PickupKey()
    {
        InventoryManager.Instance.AddKey();

        if (keyPickupSound != null) AudioSource.PlayClipAtPoint(keyPickupSound, transform.position, 0.4f);

        Destroy(gameObject);
    }
}