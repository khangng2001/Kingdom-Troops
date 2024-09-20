using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    public string PlayerName;
    public int MaxHealth;
    public int Damage;
    public int MaxStamina;
}
