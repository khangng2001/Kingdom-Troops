using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : Singleton<InventoryUIManager>
{
    [Header("Potion")]
    [SerializeField] private Button potionSwitchBtn;
    [SerializeField] private GameObject potionInventory;
    [SerializeField] private GameObject potionNotActive;

    [Space(7), Header("Weapon")]
    [SerializeField] private Button weaponSwitchBtn;
    [SerializeField] private GameObject weaponInventory;
    [SerializeField] private GameObject weaponNotActive;

    private void Start()
    {
        HideAllUI();

        potionSwitchBtn.onClick.AddListener(() =>
        {
            OnItemUIChosen();
        });

        weaponSwitchBtn.onClick.AddListener(() =>
        {
            OnWeaponUIChosen();
        });

        OnItemUIChosen();
    }


    private void OnItemUIChosen()
    {
        Hide(weaponInventory);
        weaponNotActive.SetActive(true);

        Show(potionInventory);
        potionNotActive.SetActive(false);
    }

    private void OnWeaponUIChosen()
    {
        Hide(potionInventory);
        potionNotActive.SetActive(true);

        Show(weaponInventory);
        weaponNotActive.SetActive(false);
    }

    private void Show(GameObject go)
    {
        go.SetActive(true);
    }

    private void Hide(GameObject go)
    {
        go.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void HideAllUI()
    {
        potionInventory.SetActive(false);
        potionNotActive.SetActive(false);

        weaponInventory.SetActive(false);
        weaponNotActive.SetActive(false);
    }
}
