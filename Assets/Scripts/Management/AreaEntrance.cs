using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start() {
        string currentTransitionName = SceneManagement.Instance != null ? SceneManagement.Instance.SceneTransitionName : string.Empty;

        if (!string.IsNullOrEmpty(currentTransitionName) && transitionName == currentTransitionName) {
            if (PlayerController.Instance != null) {
                PlayerController.Instance.transform.position = this.transform.position;
            }

            if (CameraController.Instance != null) {
                CameraController.Instance.SetPlayerCameraFollow();
            }
        }

        if (UIFade.Instance != null) {
            UIFade.Instance.FadeToClear();
        }
    }
}
