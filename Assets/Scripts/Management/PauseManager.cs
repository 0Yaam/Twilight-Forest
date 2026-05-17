using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private GameObject pausePanel;
    private Button resumeButton;
    private Button mainMenuButton;
    private bool isPaused = false;
    private const string RESUME_BUTTON_TEXT = "Button_Resume";
    private const string MAIN_MENU_BUTTON_TEXT = "Button_PauseMainMenu";

    private void Start()
    {
        EnsureEventSystem();
        ResolvePanel();
        ResolveButtons();
        RegisterButtonEvents();

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null) { return; }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOverActive) { return; }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) { return; }

        isPaused = true;
        EnsureEventSystem();
        ResolvePanel();
        ResolveButtons();
        RegisterButtonEvents();

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused) { return; }

        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        DestroyPersistentGameplayObjects();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void ResolveButtons()
    {
        ResolvePanel();

        if (pausePanel == null) { return; }

        if (resumeButton == null)
        {
            Transform resumeButtonTransform = pausePanel.transform.Find(RESUME_BUTTON_TEXT);
            if (resumeButtonTransform != null)
            {
                resumeButton = resumeButtonTransform.GetComponent<Button>();
            }
        }

        if (mainMenuButton == null)
        {
            Transform mainMenuButtonTransform = pausePanel.transform.Find(MAIN_MENU_BUTTON_TEXT);
            if (mainMenuButtonTransform != null)
            {
                mainMenuButton = mainMenuButtonTransform.GetComponent<Button>();
            }
        }
    }

    private void ResolvePanel()
    {
        if (pausePanel != null) { return; }

        Transform[] childTransforms = GetComponentsInChildren<Transform>(true);
        foreach (Transform childTransform in childTransforms)
        {
            if (childTransform.name == "Pause Panel")
            {
                pausePanel = childTransform.gameObject;
                return;
            }
        }
    }

    private void RegisterButtonEvents()
    {
        if (resumeButton != null)
        {
            resumeButton.interactable = true;
            resumeButton.onClick.RemoveListener(ResumeGame);
            resumeButton.onClick.AddListener(ResumeGame);
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
        AddObjectToDestroy(objectsToDestroy, GameOverManager.Instance);
        AddObjectToDestroy(objectsToDestroy, Instance);

        foreach (GameObject objectToDestroy in objectsToDestroy)
        {
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }

    private void AddObjectToDestroy<T>(HashSet<GameObject> objectsToDestroy, Singleton<T> singleton) where T : Singleton<T>
    {
        if (singleton == null) { return; }
        objectsToDestroy.Add(singleton.gameObject);
    }
}
