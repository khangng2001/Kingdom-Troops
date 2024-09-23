using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    private enum StatType
    {
        Health,
        Stamina
    }
    [SerializeField] private StatType type;

    [SerializeField] private GameObject getStatSystemGameObject;

    [SerializeField] private Slider easeSlider;
    [SerializeField] private Slider statSlider;
    [SerializeField] private float lerpSpeed = 0.03f;

    private HealthSystem healthSystem;
    private StaminaSystem staminaSystem;

    private void Awake()
    {
        getStatSystemGameObject = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Start()
    {
        if (type == StatType.Health)
            if (HealthSystem.TryGetHealthSystem(getStatSystemGameObject, out HealthSystem healthSystem))
                SetHealthSystem(healthSystem);

        if (type == StatType.Stamina)
            if (StaminaSystem.TryGetStaminaSystem(getStatSystemGameObject, out StaminaSystem staminaSystem))
                SetStaminaSystem(staminaSystem);
    }

    private void FixedUpdate()
    {
        if (type == StatType.Health)
            if (healthSystem.GetHealth() != easeSlider.value)
                easeSlider.value = Mathf.Lerp(easeSlider.value, healthSystem.GetHealth(), lerpSpeed);

        if (type == StatType.Stamina)
            if (staminaSystem.GetStamina() != easeSlider.value)
                easeSlider.value = Mathf.Lerp(easeSlider.value, staminaSystem.GetStamina(), lerpSpeed);
    }

    #region RELATE HEALTH

    private void SetHealthSystem(HealthSystem healthSystem)
    {
        if (this.healthSystem != null)
            this.healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;

        this.healthSystem = healthSystem;

        UpdateHealthBar();

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        statSlider.maxValue = healthSystem.GetHealthMax();
        easeSlider.maxValue = healthSystem.GetHealthMax();

        float health;
        health = healthSystem.GetHealth();

        if (statSlider.value != health)
            statSlider.value = health;
    }

    #endregion

    #region RELATE STAMINA

    private void SetStaminaSystem(StaminaSystem staminaSystem)
    {
        if (this.staminaSystem != null)
            this.staminaSystem.OnStaminaChanged -= StaminaSystem_OnStaminaChanged;

        this.staminaSystem = staminaSystem;

        UpdateStaminaBar();

        this.staminaSystem.OnStaminaChanged += StaminaSystem_OnStaminaChanged;
    }

    private void StaminaSystem_OnStaminaChanged(object sender, System.EventArgs e)
    {
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        statSlider.maxValue = staminaSystem.GetStaminaMax();
        easeSlider.maxValue = staminaSystem.GetStaminaMax();

        float stamina;
        stamina = staminaSystem.GetStamina();

        if (statSlider.value != stamina)
            statSlider.value = stamina;
    }

    #endregion

    // Clean up events when this Game Object is destroyed
    private void OnDestroy()
    {
        if (type == StatType.Health)
            healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;

        if (type == StatType.Stamina)
            staminaSystem.OnStaminaChanged -= StaminaSystem_OnStaminaChanged;
    }
}