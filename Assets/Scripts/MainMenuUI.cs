using UnityEngine;

/// <summary>
/// Controls the main menu panel, hiding it on the first game start event.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Tooltip("Assign the root GameObject of your main menu UI panel here.")]
    [SerializeField] private GameObject menuPanel;

    private void Awake()
    {
        // Ensure menu is visible at start
        if (menuPanel != null)
            menuPanel.SetActive(true);
    }

    private void OnEnable()
    {
        EventManager.OnGameStart += HideMenu;
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= HideMenu;
    }

    private void Start()
    {
        LeanTween.scale(menuPanel, Vector3.one * 1.1f, 0.7f)
         .setLoopPingPong()
         .setEaseInOutSine();
    }

    private void HideMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }
}
