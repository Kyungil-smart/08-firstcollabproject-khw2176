using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float lifeTime = 1f;

    private float timer;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(string message)
    {
        textUI.text = message;
        timer = 0f;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if (canvasGroup != null)
            canvasGroup.alpha = 1f - (timer / lifeTime);

        if (timer >= lifeTime)
            gameObject.SetActive(false);
    }
}