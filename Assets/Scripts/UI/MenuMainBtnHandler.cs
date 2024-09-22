using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMainBtnHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject onHover;

    private void Start()
    {
        onHover.SetActive(false);
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show(onHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide(onHover);
    }

    private void Show(GameObject go)
    {
        go.SetActive(true);
    }

    private void Hide(GameObject go)
    {
        go.SetActive(false);
    }
}
