using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : Singleton<GameOverManager>
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool gameOverActive = false;
    private const string RESTART_BUTTON_TEXT = "Button_Restart";
    private const string MAIN_MENU_BUTTON_TEXT = "Button_MainMenu";

    public bool IsGameOverActive { get { return gameOverActive; } }

    private void Start()
    {
        Time.timeScale = 1f;
        EnsureEventSystem();
        ResolveButtons();
        RegisterButtonEvents();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverActive) { return; }

        gameOverActive = true;
        EnsureEventSystem();
        ResolveButtons();
        RegisterButtonEvents();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.enabled = false;
        }

        if (ActiveWeapon.Instance != null)
        {
            ActiveWeapon.Instance.enabled = false;
            ActiveWeapon.Instance.gameObject.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadSceneAfterCleanup(currentSceneName);
    }

    public void QuitToMainMenu()
    {
        LoadSceneAfterCleanup(mainMenuSceneName);
    }

    private void LoadSceneAfterCleanup(string sceneName)
    {
        Time.timeScale = 1f;
        DestroyPersistentGameplayObjects();
        SceneManager.LoadScene(sceneName);
    }

    private void DestroyPersistentGameplayObjects()
    {
        HashSet<GameObject> objectsToDestroy = new HashSet<GameObject>();

        AddObjectToDestroy(objectsToDestroy, PlayerController.Instance);
        AddObjectToDestroy(objectsToDestroy, PlayerHealth.Instance);
        AddObjectToDestroy(objectsToDestroy, ActiveWeapon.Instance);
        AddObjectToDestroy(objectsToDestroy, Stamina.Instance);
        AddObjectToDestroy(objectsToDestroy, BaseSingleton.Instance);
        AddObjectToDestroy(objectsToDestroy, UIFade.Instance);
        AddObjectToDestroy(objectsToDestroy, ObjectiveManager.Instance);
        AddObjectToDestroy(objectsToDestroy, ScreenShakeManager.Instance);
        AddObjectToDestroy(objectsToDestroy, SceneManagement.Instance);
        AddObjectToDestroy(objectsToDestroy, CameraController.Instance);
        AddObjectToDestroy(objectsToDestroy, EconomyManager.Instance);
        AddObjectToDestroy(objectsToDestroy, AudioManager.Instance);
        AddObjectToDestroy(objectsToDestroy, Instance);

        foreach (GameObject objectToDestroy in objectsToDestroy)
        {
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }

    private void ResolveButtons()
    {
        if (gameOverPanel == null) { return; }

        if (restartButton == null)
        {
            Transform restartButtonTransform = gameOverPanel.transform.Find(RESTART_BUTTON_TEXT);
            if (restartButtonTransform != null)
            {
                restartButton = restartButtonTransform.GetComponent<Button>();
            }
        }

        if (mainMenuButton == null)
        {
            Transform mainMenuButtonTransform = gameOverPanel.transform.Find(MAIN_MENU_BUTTON_TEXT);
            if (mainMenuButtonTransform != null)
            {
                mainMenuButton = mainMenuButtonTransform.GetComponent<Button>();
            }
        }
    }

    private void RegisterButtonEvents()
    {
        if (restartButton != null)
        {
            restartButton.interactable = true;
            restartButton.onClick.RemoveListener(RestartGame);
            restartButton.onClick.AddListener(RestartGame);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.interactable = true;
            mainMenuButton.onClick.RemoveListener(QuitToMainMenu);
            mainMenuButton.onClick.AddListener(QuitToMainMenu);
        }
    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null) { return; }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<InputSystemUIInputModule>();
    }

    private void AddObjectToDestroy<T>(HashSet<GameObject> objectsToDestroy, Singleton<T> singleton) where T : Singleton<T>
    {
        if (singleton == null) { return; }
        objectsToDestroy.Add(singleton.gameObject);
    }
}
