using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private bool requiresObjectiveCompletion = false;
    [SerializeField] private GameObject lockedPortalVisual;
    [SerializeField] private GameObject unlockedPortalVisual;

    private float waitToLoadTime = 1f;
    private bool isTransitioning = false;

    private void Start() {
        UpdatePortalVisual();
    }

    private void Update() {
        UpdatePortalVisual();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isTransitioning) { return; }
        if (other.gameObject.GetComponent<PlayerController>() == null) { return; }
        if (requiresObjectiveCompletion) {
            if (ObjectiveManager.Instance == null) {
                Debug.LogWarning("AreaExit requires objective completion, but ObjectiveManager is missing.");
                return;
            }

            if (!ObjectiveManager.Instance.IsObjectiveComplete) {
                ObjectiveManager.Instance.ShowPortalLockedMessage();
                return;
            }
        }

        isTransitioning = true;
        if (SceneManagement.Instance != null) {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
        }
        if (UIFade.Instance != null) {
            UIFade.Instance.FadeToBlack();
        }
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine() {
        while (waitToLoadTime >=0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    private void UpdatePortalVisual() {
        if (!requiresObjectiveCompletion) { return; }

        bool isUnlocked = ObjectiveManager.Instance != null && ObjectiveManager.Instance.IsObjectiveComplete;

        if (lockedPortalVisual != null) {
            lockedPortalVisual.SetActive(!isUnlocked);
        }

        if (unlockedPortalVisual != null) {
            unlockedPortalVisual.SetActive(isUnlocked);
        }
    }
}
