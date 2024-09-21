using System;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealed;
    public event EventHandler OnDamaged;
    public event EventHandler OnDead;

    private int healthMax;
    private int health;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealthMax()
    {
        return healthMax;
    }
    public int GetHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    // Set the current Health amount, doesn't set above health HealthMax amount or below 0
    public void SetHealth(int health)
    {
        if (health > healthMax)
            health = healthMax;

        if (health < 0)
            health = 0;

        this.health = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
    }

    // Health with particular Amount
    public void Heal(int healthAmount)
    {
        health += healthAmount;
        if (health > healthMax) 
            health = healthMax;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    // Health to Maximum
    public void HealComplete()
    {
        health = healthMax;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    // Damage with Particular Amount
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    // Try to get a HealthSystem from the GameObject
    public static bool TryGetHealthSystem(GameObject getHealthSystemGameObject, out HealthSystem healthSystem, bool logErrors = false)
    {
        healthSystem = null;

        if (getHealthSystemGameObject != null)
        {
            if (getHealthSystemGameObject.TryGetComponent(out IGetHealthSystem getHealthSystem))
            {
                healthSystem = getHealthSystem.GetHealthSystem();
                if (healthSystem != null)
                {
                    return true;
                }
                else
                {
                    if (logErrors)
                    {
                        Debug.LogError($"Got HealthSystem from object but healthSystem is null! Should it have been created? Maybe you have an issue with the order of operations.");
                    }
                    return false;
                }
            }
            else
            {
                if (logErrors)
                {
                    Debug.LogError($"Referenced GameObject '{getHealthSystemGameObject}' does not have a script that implements IGetHealthSystem!");
                }
                return false;
            }
        }
        else
        {
            // No reference assigned
            if (logErrors)
            {
                Debug.LogError($"You need to assign the field 'getHealthSystemGameObject'!");
            }
            return false;
        }
    }
}