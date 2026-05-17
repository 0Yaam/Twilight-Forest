using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;
    private bool hasWarnedMissingGoldText = false;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";
    const string GOLD_COIN_CONTAINER_TEXT = "Gold Coin Container";

    public void UpdateCurrentGold() {
        currentGold += 1;

        if (!TryResolveGoldText()) {
            if (!hasWarnedMissingGoldText) {
                Debug.LogWarning("EconomyManager could not find 'Gold Amount Text' with TMP_Text in the current scene.");
                hasWarnedMissingGoldText = true;
            }
            return;
        }

        goldText.text = currentGold.ToString("D3");
    }

    private bool TryResolveGoldText() {
        if (goldText != null) {
            return true;
        }

        GameObject goldTextObject = GameObject.Find(COIN_AMOUNT_TEXT);
        if (goldTextObject != null) {
            goldText = goldTextObject.GetComponent<TMP_Text>();
            if (goldText != null) {
                return true;
            }
        }

        GameObject goldContainerObject = GameObject.Find(GOLD_COIN_CONTAINER_TEXT);
        if (goldContainerObject != null) {
            goldText = goldContainerObject.GetComponentInChildren<TMP_Text>(true);
            if (goldText != null) {
                return true;
            }
        }

        return goldText != null;
    }
}
