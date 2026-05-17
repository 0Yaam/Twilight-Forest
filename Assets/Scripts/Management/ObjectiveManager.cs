using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : Singleton<ObjectiveManager>
{
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private bool autoSetTargetFromSceneEnemies = true;
    [SerializeField] private int targetEnemiesDefeated = 0;
    [SerializeField] private string objectivePrefix = "Defeat enemies";
    [SerializeField] private string objectiveCompleteText = "Portal opened";
    [SerializeField] private string objectiveLockedText = "Defeat all enemies first";
    [SerializeField] private float temporaryMessageTime = 1.5f;

    private int enemiesDefeated = 0;
    private Coroutine temporaryMessageRoutine;

    public bool IsObjectiveComplete { get; private set; }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ResetObjective();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ResetObjectiveRoutine());
    }

    private IEnumerator ResetObjectiveRoutine()
    {
        yield return null;
        ResetObjective();
    }

    private void ResetObjective()
    {
        enemiesDefeated = 0;

        if (autoSetTargetFromSceneEnemies)
        {
            targetEnemiesDefeated = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None).Length;
        }

        UpdateObjectiveState();
        UpdateObjectiveText();
    }

    public void RegisterEnemyDefeated()
    {
        if (IsObjectiveComplete) { return; }

        enemiesDefeated++;
        UpdateObjectiveState();
        UpdateObjectiveText();
    }

    public void ShowPortalLockedMessage()
    {
        if (objectiveText == null) { return; }

        if (temporaryMessageRoutine != null)
        {
            StopCoroutine(temporaryMessageRoutine);
        }

        temporaryMessageRoutine = StartCoroutine(TemporaryMessageRoutine(objectiveLockedText));
    }

    private void UpdateObjectiveState()
    {
        IsObjectiveComplete = targetEnemiesDefeated <= 0 || enemiesDefeated >= targetEnemiesDefeated;
    }

    private void UpdateObjectiveText()
    {
        if (objectiveText == null) { return; }

        if (IsObjectiveComplete)
        {
            objectiveText.text = objectiveCompleteText;
            return;
        }

        objectiveText.text = $"{objectivePrefix}: {enemiesDefeated}/{targetEnemiesDefeated}";
    }

    private IEnumerator TemporaryMessageRoutine(string message)
    {
        string previousText = objectiveText.text;
        objectiveText.text = message;

        yield return new WaitForSeconds(temporaryMessageTime);

        objectiveText.text = previousText;
        temporaryMessageRoutine = null;
    }
}
