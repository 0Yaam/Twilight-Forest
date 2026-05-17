using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Transform heartContainer;

    [SerializeField] private Slider healthSlider;
    [Header("Low Health Warning")]
    [SerializeField][Min(1)] private int lowHealthThreshold = 1;
    [SerializeField][Min(0.05f)] private float lowHealthBlinkInterval = 0.2f;
    [SerializeField] private Color lowHealthFlashColor = Color.white;

    private int currentHealth;
    private bool canTakeDamage = true;
    private bool isDead = false;
    private Knockback knockback;
    private Flash flash;
    private Image healthFillImage;
    private Color normalHealthFillColor = Color.white;
    private bool hasNormalHealthFillColor = false;
    private Coroutine lowHealthWarningRoutine;
    const string HEART_CONTAINER_TEXT = "Heart Container";
    const string HEART_SLIDER_TEXT = "Heart Slider";

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthSlider();
        UpdateLowHealthWarning();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponentInParent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
            UpdateLowHealthWarning();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (isDead) { return; }
        if (!canTakeDamage) { return; }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerHit();
        }

        if (ScreenShakeManager.Instance != null)
        {
            ScreenShakeManager.Instance.ShakeScreen();
        }

        if (knockback != null)
        {
            knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }

        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        UpdateLowHealthWarning();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            StopLowHealthWarning();
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.ShowGameOver();
            }
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (heartContainer == null)
        {
            GameObject heartContainerObject = GameObject.Find(HEART_CONTAINER_TEXT);
            if (heartContainerObject != null)
            {
                heartContainer = heartContainerObject.transform;
            }
        }

        if (healthSlider == null)
        {
            if (heartContainer != null)
            {
                healthSlider = heartContainer.GetComponentInChildren<Slider>(true);
            }
        }

        if (healthSlider == null)
        {
            GameObject heartSliderObject = GameObject.Find(HEART_SLIDER_TEXT);
            if (heartSliderObject != null)
            {
                healthSlider = heartSliderObject.GetComponent<Slider>();
            }
        }

        if (healthSlider == null)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        // Fix for health bar spilling out of container
        if (healthSlider.fillRect != null)
        {
            healthSlider.fillRect.offsetMax = new Vector2(0, healthSlider.fillRect.offsetMax.y); // Set Right Stretch to 0
            healthSlider.fillRect.offsetMin = new Vector2(0, healthSlider.fillRect.offsetMin.y); // Set Left Stretch to 0
        }

        ResolveHealthFillImage();
    }

    private void ResolveHealthFillImage()
    {
        if (healthFillImage != null) { return; }
        if (healthSlider == null || healthSlider.fillRect == null) { return; }

        healthFillImage = healthSlider.fillRect.GetComponent<Image>();
        if (healthFillImage == null) { return; }

        normalHealthFillColor = healthFillImage.color;
        hasNormalHealthFillColor = true;
    }

    private void UpdateLowHealthWarning()
    {
        ResolveHealthFillImage();

        bool shouldWarn = currentHealth > 0 && currentHealth <= lowHealthThreshold && !isDead;
        if (shouldWarn)
        {
            if (lowHealthWarningRoutine == null)
            {
                lowHealthWarningRoutine = StartCoroutine(LowHealthWarningRoutine());
            }
            return;
        }

        StopLowHealthWarning();
    }

    private IEnumerator LowHealthWarningRoutine()
    {
        while (true)
        {
            SetHealthFillColor(lowHealthFlashColor);
            yield return new WaitForSeconds(lowHealthBlinkInterval);

            RestoreHealthFillColor();
            yield return new WaitForSeconds(lowHealthBlinkInterval);
        }
    }

    private void StopLowHealthWarning()
    {
        if (lowHealthWarningRoutine != null)
        {
            StopCoroutine(lowHealthWarningRoutine);
            lowHealthWarningRoutine = null;
        }

        RestoreHealthFillColor();
    }

    private void SetHealthFillColor(Color color)
    {
        if (healthFillImage == null) { return; }

        healthFillImage.color = color;
    }

    private void RestoreHealthFillColor()
    {
        if (healthFillImage == null || !hasNormalHealthFillColor) { return; }

        healthFillImage.color = normalHealthFillColor;
    }

    private void OnDisable()
    {
        StopLowHealthWarning();
    }
}
