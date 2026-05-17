using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    [Header("Skin")]
    [SerializeField] private TMP_Text skinNameText;
    [SerializeField] private Button nextSkinButton;
    [SerializeField] private Button previousSkinButton;

    private const string SKIN_NAME_TEXT = "Text_SkinName";
    private const string NEXT_SKIN_BUTTON_TEXT = "Button_NextSkin";
    private const string PREVIOUS_SKIN_BUTTON_TEXT = "Button_PreviousSkin";

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ResolvePanels();
        RemoveLegacyAutoLabels();
        ResolveSkinText();
        ResolveSkinButtons();
        RegisterSkinButtonEvents();
        RefreshSkinText();

        if (creditsPanel != null)
            creditsPanel.SetActive(false);
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureMainMenuManager()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu") { return; }
        if (FindAnyObjectByType<MainMenuManager>() != null) { return; }

        GameObject managerObject = GameObject.Find("MainMenuManager");
        if (managerObject == null)
        {
            managerObject = new GameObject("MainMenuManager");
        }

        managerObject.AddComponent<MainMenuManager>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void OpenCredits()
    {
        if (creditsPanel == null) { return; }
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel == null) { return; }
        creditsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void NextSkin()
    {
        PlayerSkin.SelectNextSkin();
        RefreshSkinText();
    }

    public void PreviousSkin()
    {
        PlayerSkin.SelectPreviousSkin();
        RefreshSkinText();
    }

    private void ResolvePanels()
    {
        if (creditsPanel == null)
        {
            GameObject credits = GameObject.Find("Panel_Credits");
            if (credits != null) { creditsPanel = credits; }
        }

        if (settingsPanel == null)
        {
            GameObject settings = GameObject.Find("Panel_Settings");
            if (settings != null) { settingsPanel = settings; }
        }
    }

    private void ResolveSkinText()
    {
        if (skinNameText != null) { return; }

        GameObject skinTextObject = GameObject.Find(SKIN_NAME_TEXT);
        if (skinTextObject != null)
        {
            skinNameText = skinTextObject.GetComponent<TMP_Text>();
        }
    }

    private void ResolveSkinButtons()
    {
        if (nextSkinButton == null)
        {
            GameObject nextButtonObject = GameObject.Find(NEXT_SKIN_BUTTON_TEXT);
            if (nextButtonObject != null)
            {
                nextSkinButton = nextButtonObject.GetComponent<Button>();
            }
        }

        if (previousSkinButton == null)
        {
            GameObject previousButtonObject = GameObject.Find(PREVIOUS_SKIN_BUTTON_TEXT);
            if (previousButtonObject != null)
            {
                previousSkinButton = previousButtonObject.GetComponent<Button>();
            }
        }
    }

    private void RegisterSkinButtonEvents()
    {
        if (nextSkinButton != null)
        {
            nextSkinButton.onClick.RemoveListener(NextSkin);
            nextSkinButton.onClick.AddListener(NextSkin);
        }

        if (previousSkinButton != null)
        {
            previousSkinButton.onClick.RemoveListener(PreviousSkin);
            previousSkinButton.onClick.AddListener(PreviousSkin);
        }
    }

    private void RefreshSkinText()
    {
        if (skinNameText == null) { return; }

        skinNameText.text = PlayerSkin.GetSelectedSkinName();
    }

    private void RemoveLegacyAutoLabels()
    {
        Transform[] allTransforms = FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Transform currentTransform in allTransforms)
        {
            if (currentTransform.name != "Label") { continue; }
            if (currentTransform.parent == null) { continue; }
            if (!currentTransform.parent.name.StartsWith("Button_")) { continue; }

            Destroy(currentTransform.gameObject);
        }
    }
}
