
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UseButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{

    public bool IsDown = false;
    public bool IsUsingCard = false;
    [SerializeField] private Image image;
    [SerializeField] Button cancelButton;
    [SerializeField] Button removeButton;
    [SerializeField] Color offColour;
    private void Update()
    {

        if (MyCursor.Instance.GetIsActionable() && IsUsingCard)
        {
            image.color = Color.white;

            if (IsDown)
            {
                GameController.Instance.UseSelectedCard();
            }
        }
        else
        {
            image.color = offColour;         
        }

        if (IsUsingCard)
        {
            cancelButton.interactable = true;
            removeButton.interactable = true;
        }
        else
        {
            cancelButton.interactable = false;
            removeButton.interactable = false;
        }

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        IsDown = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        IsDown = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        GameController.Instance.UseSelectedCard();
    }
}
