using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake() {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start() {
        GameObject staminaContainerObject = GameObject.Find(STAMINA_CONTAINER_TEXT);
        if (staminaContainerObject != null) {
            staminaContainer = staminaContainerObject.transform;
        }

        HandleStaminaChanged();
    }

    public void UseStamina() {
        if (CurrentStamina <= 0) { return; }

        CurrentStamina--;
        HandleStaminaChanged();
    }

    public void RefreshStamina() {
        if (CurrentStamina < maxStamina) {
            CurrentStamina++;
        }
        HandleStaminaChanged();
    }

    private IEnumerator RefreshStaminaRoutine() {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void HandleStaminaChanged() {
        UpdateStaminaImages();

        if (CurrentStamina < maxStamina) {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }

    private void UpdateStaminaImages() {
        if (staminaContainer == null || fullStaminaImage == null || emptyStaminaImage == null) {
            return;
        }

        if (staminaContainer.childCount < maxStamina) {
            return;
        }

        for (int i = 0; i < maxStamina; i++)
        {
            Image staminaImage = staminaContainer.GetChild(i).GetComponent<Image>();
            if (staminaImage == null) {
                continue;
            }

            if (i <= CurrentStamina - 1) {
                staminaImage.sprite = fullStaminaImage;
            } else {
                staminaImage.sprite = emptyStaminaImage;
            }
        }
    }
}
