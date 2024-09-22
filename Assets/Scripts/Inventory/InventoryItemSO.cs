using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "SO/InventoryItemSO")]
public class InventoryItemSO : ScriptableObject
{
    [Header("Stacking")]
    public bool stackable;
    public int maxStacking;

    [Space(5), Header("UI")]
    public string id;
    public Sprite image;

    [Space(5), Header("Stat")]
    public int health;
    public int stamina;
    public int dmg;

    [Space(5), Header("Des")]
    public string description;
}