using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcID;
    [SerializeField] private string npcName;

    public float interactRange = 3f;
    public Vector3 textOffset = new Vector3(0, 2.2f, 0);

    public GameObject interactTextPrefab;

    private Transform player;
    private GameObject interactInstance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (interactTextPrefab != null)
        {
            interactInstance = Instantiate(interactTextPrefab, transform);
            interactInstance.transform.localPosition = textOffset;
            interactInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactRange)
        {
            if (interactInstance != null) interactInstance.SetActive(true);

            if (interactInstance != null)
            {
                interactInstance.transform.LookAt(player);
                interactInstance.transform.Rotate(0, 180, 0);
            }

            if (Input.GetKeyDown(KeyCode.E)) StartDialogue();
        }
        else
        {
            if (interactInstance != null) interactInstance.SetActive(false);
        }
    }

    public void SetNPC(string id, string name)
    {
        npcID = id;
        npcName = name;
    }

    void StartDialogue()
    {
        DialogueController.Instance.StartDialogue(npcID, npcName);
    }
}