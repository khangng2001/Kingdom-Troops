using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemAmount;

    private GameObject itemDes;
    private TextMeshProUGUI itemDesText;

    private InventoryItemSO item;
    private int amount;

    public int Amount { get =>  amount; set => amount = value; }

    private void Start()
    {
        itemDes.SetActive(false);
    }


    public void InitialiseItem(InventoryItemSO item, GameObject itemDes)
    {
        this.item = item;
        itemImg.sprite = item.image;

        this.itemDes = itemDes;
        itemDesText = itemDes.GetComponentInChildren<TextMeshProUGUI>();

        RefreshAmount();
    }
        

    public void RefreshAmount()
    {
        itemAmount.text = amount.ToString();

        //Show text if Amount > 1
        bool textAmountActive = amount > 1;
        itemAmount.gameObject.SetActive(textAmountActive);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDesText.text = item.description;

        itemDes.transform.position = eventData.position + new Vector2(0, 35);
        itemDes.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDes.SetActive(false);
    }
}
