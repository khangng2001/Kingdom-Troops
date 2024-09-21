using System;
using UnityEngine;

public class StaminaSystem
{
    public event EventHandler OnStaminaChanged;
    public event EventHandler OnRecovered;
    public event EventHandler OnUsed;

    private int staminaMax;
    private int stamina;

    public StaminaSystem(int staminaMax)
    {
        this.staminaMax = staminaMax;
        stamina = staminaMax;
    }

    public int GetStaminaMax()
    {
        return staminaMax;
    }
    public int GetStamina()
    {
        return stamina;
    }

    // Check Character is enough Stamina or not to Attack, Skill...
    public bool IsEnoughStamina(int staminaNeed)
    {
        return stamina > staminaNeed;
    }

    // Set the current Health amount, doesn't set above health HealthMax amount or below 0
    public void SetStamina(int stamina)
    {
        if (stamina > staminaMax)
            stamina = staminaMax;

        if (stamina < 0)
            stamina = 0;

        this.stamina = stamina;
        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
    }

    // Recover with particular Amount
    public void Recover(int staminaAmount)
    {
        stamina += staminaAmount;
        if (stamina > staminaMax)
            stamina = staminaMax;

        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        OnRecovered?.Invoke(this, EventArgs.Empty);
    }

    // Recover to Maximum
    public void RecoverComplete()
    {
        stamina = staminaMax;

        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        OnRecovered?.Invoke(this, EventArgs.Empty);
    }

    public void UseStamina(int staminaAmount)
    {
        stamina -= staminaAmount;

        if (stamina < 0)
            stamina = 0;

        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        OnUsed?.Invoke(this, EventArgs.Empty);
    }


    // Try to get a HealthSystem from the GameObject
    public static bool TryGetStaminaSystem(GameObject getStaminaSystemGameObject, out StaminaSystem staminaSystem, bool logErrors = false)
    {
        staminaSystem = null;

        if (getStaminaSystemGameObject != null)
        {
            if (getStaminaSystemGameObject.TryGetComponent(out IGetStaminaSystem getStaminaSystem))
            {
                staminaSystem = getStaminaSystem.GetStaminaSystem();
                if (staminaSystem != null)
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
                    Debug.LogError($"Referenced Game Object '{getStaminaSystemGameObject}' does not have a script that implements IGetHealthSystem!");
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
