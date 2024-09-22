using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("Inventory Item")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform potionParent;
    [SerializeField] private Transform weaponParent;
    [SerializeField] private GameObject itemDes;

    #region ONLY FOR TEST (FAKE DATA)

    public List<InventoryItemSO> potionSOs = new List<InventoryItemSO>();
    public List<InventoryItemSO> waeponSOs = new List<InventoryItemSO>();


    //Test DATA
    public List<ItemDetail> potions = new List<ItemDetail>();
    public List<ItemDetail> weapons = new List<ItemDetail>();

    private GameData gameData;
    #endregion

    private void Start()
    {
        InventoryUIManager.Instance.Hide();

        gameData = DataManager.Instance.LoadGame();

        #region TEST
        foreach (InventoryItemSO item in potionSOs)
        {
            SpawnPotion(item, 3);
        }

        foreach (InventoryItemSO item in waeponSOs)
        {
            SpawnWeapon(item, 1);
        }
        #endregion
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryUIManager.Instance.gameObject.activeInHierarchy)
                InventoryUIManager.Instance.Hide();
            else
                InventoryUIManager.Instance.Show();
        }
    }



    // Get Data (All Item, Weapon Player has)


    // Spawn Items in Inventory
    public void SpawnPotion(InventoryItemSO itemSO, int amount)
    {
        GameObject item = Instantiate(itemPrefab, potionParent);

        //Set Detail to Item
        InventoryItem inventoryItem = item.GetComponent<InventoryItem>();
        inventoryItem.Amount = amount;
        inventoryItem.InitialiseItem(itemSO, itemDes);
    }

    public void SpawnWeapon(InventoryItemSO itemSO, int amount)
    {
        GameObject item = Instantiate(itemPrefab, weaponParent);

        //Set Detail to Item
        InventoryItem inventoryItem = item.GetComponent<InventoryItem>();
        inventoryItem.Amount = amount;
        inventoryItem.InitialiseItem(itemSO, itemDes);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}