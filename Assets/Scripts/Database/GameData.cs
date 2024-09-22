using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemDetail
{
    public string ID;
    public int Amount;

    public ItemDetail(string id, int amount)
    {
        ID = id;
        Amount = amount;
    }
}

[Serializable]
public class CharacterStat
{
    public int Health;
    public int Stamina;

    public CharacterStat(int health, int stamina)
    {
        Health = health;
        Stamina = stamina;
    }
}

public class GameData
{
    public int AppOpenCounts = 0;
    public bool NotPlayedEver = true;

    public string CurLevelMap = "";
    public string CurWeaponEquip = "";

    public List<ItemDetail> Potions = new List<ItemDetail>();
    public List<ItemDetail> Weapons = new List<ItemDetail>();

    public CharacterStat CharacterStat = new CharacterStat(100, 100);

    public void Init()
    {
        if (AppOpenCounts <= 0)
        {
            Reset();
            NotPlayedEver = true;
        }

        AppOpenCounts++;

        DataManager.Instance.SaveGame();
    }

    public void Reset()
    {
        CurLevelMap = StringConstants.LEVEL_01;
        CurWeaponEquip = "";

        Potions.Clear();
        Weapons.Clear();

        CharacterStat = new CharacterStat(100, 100);
    }


    #region INVENTORY ITEM

    public void UpdatePotions(List<ItemDetail> potions)
    {
        Potions = potions;

        DataManager.Instance.SaveGame();
    }

    public void UpdateWeapons(List<ItemDetail> weapons)
    {
        Weapons = weapons;

        DataManager.Instance.SaveGame();
    }

    #endregion

    #region CHARACTER STAT

    public void UpdateStat(int health, int stamina)
    {
        CharacterStat.Health = health;
        CharacterStat.Stamina = stamina;

        DataManager.Instance.SaveGame();
    }

    #endregion

    #region OTHERS

    public void UpdateNotPlayedEver(bool b)
    {
        NotPlayedEver = b;

        DataManager.Instance.SaveGame();
    }

    public void UpdateCurWeaponEquip(string curWeaponEquip)
    {
        CurWeaponEquip = curWeaponEquip;

        DataManager.Instance.SaveGame();
    }

    public void UpdateCurLevelMap(string curLevelMap)
    {
        CurLevelMap = curLevelMap;

        DataManager.Instance.SaveGame();
    }

    #endregion
}
