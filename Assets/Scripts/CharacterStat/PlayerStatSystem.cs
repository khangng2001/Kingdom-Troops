using UnityEngine;

public class PlayerStatSystem : MonoBehaviour, IGetHealthSystem, IGetStaminaSystem
{
    [SerializeField] private int healthMax = 100;
    [SerializeField] private int staminaMax = 20;

    private HealthSystem healthSystem;
    private StaminaSystem staminaSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(healthMax);
        staminaSystem = new StaminaSystem(staminaMax);

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    #region RELATE HEALTH

    public void Healing(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    public void Takedamage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Debug.Log("PLAYER DIED");
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
