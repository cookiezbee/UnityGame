using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    CanvasGroup cg;
    float t;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;
    }

    void Update()
    {
        t += Time.unscaledDeltaTime / duration;
        cg.alpha = Mathf.Clamp01(t);
    }
}