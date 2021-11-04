using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * A button that can be held down to activate a selected action card
 * Also toggles the cancel button (which deselects a card/cancels an action) and the discard button (which discards/destroys a selected card)
 *@ author Manny Kwong 
 */

public class UseButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{

    public bool IsDown = false;
    public bool IsUsingCard = false;
    [SerializeField] private Image image;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private Color offColour; //Colour when off/deactivated

    private void Update()
    {
        //Activate selected card if meets conditions otherwise deactivate
        if (GameController.Instance.GetIsCardUsable())
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

        //If a card is selected/being used allow the cancel and discard buttons to be used
        if (IsUsingCard)
        {
            cancelButton.interactable = true;
            discardButton.interactable = true;
        }
        else
        {
            cancelButton.interactable = false;
            discardButton.interactable = false;
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
