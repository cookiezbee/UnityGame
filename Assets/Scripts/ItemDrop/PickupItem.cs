using UnityEngine;
using TMPro;

public abstract class PickupItem : MonoBehaviour
{
    public float pickupRange = 2f;
    public Vector3 textOffset = new Vector3(0, 1.5f, 0);

    public GameObject pickupTextPrefab;

    protected Transform player;
    private GameObject textInstance;
    private bool isNearPlayer = false;
    private TextMeshProUGUI tmpText;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (pickupTextPrefab != null)
        {
            textInstance = Instantiate(pickupTextPrefab, transform);
            textInstance.transform.localPosition = textOffset;

            tmpText = textInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null) tmpText.text = GetPickupText();

            textInstance.SetActive(false);
        }
    }

    protected virtual void Update()
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

        if (isNearPlayer && Input.GetKeyDown(KeyCode.E)) TryPickup();
    }

    private void TryPickup()
    {
        bool success = OnPickup(player.gameObject);

        if (success) Destroy(gameObject);
        else ShowWarning();
    }

    private void ShowWarning()
    {
        if (tmpText != null)
        {
            string originalText = tmpText.text;
            tmpText.text = GetWarningText();
            tmpText.color = Color.red;

            CancelInvoke(nameof(ResetText));
            Invoke(nameof(ResetText), 2f);
        }
    }

    private void ResetText()
    {
        if (tmpText != null)
        {
            tmpText.text = GetPickupText();
            tmpText.color = Color.white;
        }
    }

    protected abstract bool OnPickup(GameObject player);

    protected abstract string GetPickupText();

    protected abstract string GetWarningText();
}
