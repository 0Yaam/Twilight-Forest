using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ResolvePanels();
        RemoveLegacyAutoLabels();

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
