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
    private bool _isUsingCard = false;
    private bool _isOverSelectable = false;

    [SerializeField] private Image image;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private Color offColour; //Colour when off/deactivated
    [SerializeField] private Slider buildingRotationSlider;

    private void Update()
    {
        //Activate selected card if meets conditions otherwise deactivate
        if (_isUsingCard)
        {
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
        }

       
        

        if (!GameController.Instance.IsAR)
        {
            PCControls();
        }
    }



    //If a selectable object is hovered over, make button interactable 
    public void SetHoveringSelectable(ISelectable selectable)
    {
        if (selectable != null)
        {
            _isOverSelectable = true;
            image.color = Color.white;
        }
        else
        {
            _isOverSelectable = false;
            image.color = offColour;
        }
    }



    public void SetBuildingSliderActive(bool isActive)
    {
        buildingRotationSlider.gameObject.SetActive(isActive);
    }

    public void SetIsUsingCard(Card selectedCard)
    {
        //If a card is selected/being used allow the cancel and discard buttons to be used
        if (selectedCard != null)
        {
            _isUsingCard = true;
            cancelButton.interactable = true;
            discardButton.interactable = true;

            //If card is a building card, turn on rotate building rotation slider
            if (selectedCard.GetComponent<BuildingCard>())
            {
                buildingRotationSlider.gameObject.SetActive(true);
            }
            else
            {
                buildingRotationSlider.gameObject.SetActive(false);
            }
        }
        else
        {
            _isUsingCard = false;
            cancelButton.interactable = false;
            discardButton.interactable = false;
            buildingRotationSlider.gameObject.SetActive(false);
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
        OnClick();
    }

    public void OnClick()
    {
        if (_isOverSelectable && !GameController.Instance.IsHoldingObject)
        {
            GameController.Instance.SelectObject();
            SelectObject();
        }
        else if (GameController.Instance.IsHoldingObject)
        {
            if (GameController.Instance.UseObject())
            {
                GameController.Instance.DeselectObject();
                DeselectObject();
            }
        }
    }

    public void SelectObject()
    {
        image.color = Color.white;
        cancelButton.interactable = true;
        discardButton.interactable = true;
    }

    public void DeselectObject()
    {
        image.color = offColour;
        cancelButton.interactable = false;
        discardButton.interactable = false;
    }

    private void PCControls()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnClick();
            IsDown = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsDown = true;
        }


            if (Input.mouseScrollDelta.y > 0)
        {
            if (buildingRotationSlider.value < buildingRotationSlider.maxValue)
            {
                buildingRotationSlider.value++;
            }
            else
            {
                buildingRotationSlider.value = 0;
            }
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if (buildingRotationSlider.value > buildingRotationSlider.minValue)
            {
                buildingRotationSlider.value--;
            }
            else
            {
                buildingRotationSlider.value = buildingRotationSlider.maxValue;
            }
        }

        
    }
}
