using UnityEngine;

public class FixBounds : MonoBehaviour
{
    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField] Vector3 size = new Vector3(10, 10, 10);

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.mesh != null)
        {
            Mesh mesh = meshFilter.mesh;
            mesh.bounds = new Bounds(center, size);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + center, size);
    }
}
