using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Represents the actions the player can do such as placing a building, upgrading a tower or casting miracles
 * Tap to select ability and press the use button/mouse click/tap again to activate
 *@ author Manny Kwong 
 */

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image image;
    [SerializeField] protected GameObject Ghost; //Shows a placeholder at where the ability could be activated
    [SerializeField] protected string Description = "Basic Card";
    [SerializeField] Vector3 _moveToOnSelect = Vector3.up * 20; //Moving animation for when selected
    [SerializeField] protected float _activationTileState = 100;

    public CardDeck Deck;
    private RectTransform _rectTransform;
    private float _selectTime = 0.07f; //Time for select animation
    private bool _isSelected = false;
    private bool _isActivating = false;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        //Show the card being unselected
        if (!_isSelected)
        {
            image.color = Color.gray;
        }
    }

    //Select the card when tapped the first time
    //Activate card when held while selected if AR enabled
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isSelected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
        /*
        else
        {
            if (GameController.Instance.IsAR)
            {
                ActivateCard();
            }
            _isActivating = true;
        }*/
    }

    //Deactivate card when finished hold
    public void OnPointerUp(PointerEventData eventData)
    {
        /*
        if (GameController.Instance.IsAR)
        {
            if (_isActivating)
            {
                DeactivateCard();
                _isActivating = false;
            }
        }
        */
    }

    //Highlight card when selected and move card slightly out of the hand
    public void Select()
    {
        if (GameController.Instance.IsHoldingObject)
        {
            return;
        }
        _isSelected = true;
        image.color = Color.white;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position + _moveToOnSelect, _selectTime));


        //Turn on and scale the ghost to the scale of the world
        Ghost.SetActive(true);
        Ghost.transform.SetParent(World.Instance.transform);
        Ghost.transform.localScale = Vector3.one;


        GameController.Instance.SetSelectedCard(this);

    }

    //Unhighlight card, move back into hand, turn off ghost and remove description
    public void Deselect()
    {
        _isSelected = false;
        image.color = Color.gray;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position, _selectTime));
        Ghost.SetActive(false);
        GameController.Instance.DeselectCard(this);

    }

    //Show description of card ability at the top of the screen (game info area)
    public virtual void SetGameInfo()
    {
        GameInfo.Instance.SetCardText(Description);
    }

    //Move card into position
    //Mainly used when drawing a card from deck into hand
    IEnumerator MoveCard(Vector3 moveTo, float duration)
    {
        Vector3 moveFrom = image.GetComponent<RectTransform>().position;
        for (float i = 0; i < duration; i += 0.01f)
        {
            image.GetComponent<RectTransform>().position = Vector3.Lerp(moveFrom, moveTo, i / duration);

            yield return new WaitForSeconds(0.01f);
        }
        image.GetComponent<RectTransform>().position = moveTo;
    }


    void Update()
    {
        //If selected and the action is valid on cursor target, place the ghost in front of the cursor

        if (_isSelected)
        {
            RaycastHit hit = MyCursor.Instance.GetCursorHit();
            if (Ghost != null && MyCursor.Instance.GetCursorHitting())
            {
                UpdateGhost(hit);
            }
        }

        //Move towards target location
        _rectTransform.localPosition = Vector3.MoveTowards(_rectTransform.localPosition, _targetPosition, 2000 * Time.deltaTime); ;
    }

    public bool GetIsUsable()
    {
        return MyCursor.Instance.GetTileState() >= _activationTileState;
    }

    Vector3 _targetPosition;
    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    protected virtual void UpdateGhost(RaycastHit hit)
    {
        Ghost.transform.position = hit.point;
        Ghost.transform.up = Vector3.up;
    }

    //Destroy card on discard and remove from deck/hand
    public void Discard()
    {
        GameController.Instance.DeselectCard();
        Destroy(Ghost);

        if (Deck)
        {
            Deck.DiscardCard(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual bool ActivateCard()
    {
        return true;
    }

    public virtual void DeactivateCard()
    {

    }
}
