using UnityEngine;

public class PlayerStatSystem : MonoBehaviour, IGetHealthSystem, IGetStaminaSystem
{
    //[SerializeField] private int healthMax = 100;
    //[SerializeField] private int staminaMax = 20;

    private HealthSystem healthSystem;
    private StaminaSystem staminaSystem;

    public void GetData(HealthSystem healthSystem, StaminaSystem staminaSystem)
    {
        this.healthSystem = healthSystem;
        this.staminaSystem = staminaSystem;
    }

    #region RELATE HEALTH

    public void Healing(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    public void TakeDamage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    #endregion

    #region RELATE HEALTH

    public void Recovering(int staminaAmount)
    {
        staminaSystem.Recover(staminaAmount);
    }

    public void UseStamina(int staminaAmount)
    {
        staminaSystem.UseStamina(staminaAmount);
    }

    public StaminaSystem GetStaminaSystem()
    {
        return staminaSystem;
    }

    #endregion
}
