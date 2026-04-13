using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRaycaster : MonoBehaviour
{
    [Header("레이캐스트 설정")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rayDistance = 100f;

    private Mouse mouse;

    private void Awake()
    {
        mouse = Mouse.current;
    }

    private void Update()
    {
        if (mouse == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                Debug.Log($"맞은 오브젝트: {hit.collider.name}");

                MineralClickHandler mineral = hit.collider.GetComponent<MineralClickHandler>();

                if (mineral != null)
                {
                    mineral.OnClick();
                }
                else
                {
                    Debug.Log("클릭한 오브젝트에 MineralClickHandler가 없음");
                }
            }
            else
            {
                Debug.Log("아무것도 안 맞음");
            }
        }
    }
}