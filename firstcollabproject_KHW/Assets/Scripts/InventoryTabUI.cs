using UnityEngine;
using UnityEngine.UI;

public class InventoryTabUI : MonoBehaviour
{
    [Header("시대 패널")]
    [SerializeField] private GameObject ancientPanel;
    [SerializeField] private GameObject medievalPanel;
    [SerializeField] private GameObject modernPanel;
    [SerializeField] private GameObject spacePanel;

    [Header("탭 버튼")]
    [SerializeField] private Button ancientButton;
    [SerializeField] private Button medievalButton;
    [SerializeField] private Button modernButton;
    [SerializeField] private Button spaceButton;

    private void Start()
    {
        OpenAncient();
        RefreshTabButtons();

        if (LevelManager.Instance != null)
            LevelManager.Instance.OnLevelChanged += OnLevelChanged;
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance != null)
            LevelManager.Instance.OnLevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int level)
    {
        RefreshTabButtons();
    }

    private void RefreshTabButtons()
    {
        if (LevelManager.Instance == null)
            return;

        if (ancientButton != null) ancientButton.interactable = true;
        if (medievalButton != null) medievalButton.interactable = LevelManager.Instance.CurrentLevel >= 11;
        if (modernButton != null) modernButton.interactable = LevelManager.Instance.CurrentLevel >= 21;
        if (spaceButton != null) spaceButton.interactable = LevelManager.Instance.CurrentLevel >= 31;
    }

    public void OpenAncient() => SetPanel(ancientPanel);
    public void OpenMedieval()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevel < 11)
            return;

        SetPanel(medievalPanel);
    }

    public void OpenModern()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevel < 21)
            return;

        SetPanel(modernPanel);
    }

    public void OpenSpace()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevel < 31)
            return;

        SetPanel(spacePanel);
    }

    private void SetPanel(GameObject target)
    {
        if (ancientPanel != null) ancientPanel.SetActive(target == ancientPanel);
        if (medievalPanel != null) medievalPanel.SetActive(target == medievalPanel);
        if (modernPanel != null) modernPanel.SetActive(target == modernPanel);
        if (spacePanel != null) spacePanel.SetActive(target == spacePanel);
    }
}