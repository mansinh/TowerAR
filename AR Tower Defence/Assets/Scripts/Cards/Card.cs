using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image image;
    [SerializeField] protected GameObject Ghost;  
    [SerializeField] protected string Description = "Basic Card";
    [SerializeField] Vector3 _moveToOnSelect = Vector3.up*20;
    public CardDeck Deck;
    Text _descriptionText;
    RectTransform _rectTransform;
    float _selectTime = 0.07f;
    bool _isSelected = false;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
   

        image.color = Color.gray;
        GameObject descriptionObject = GameObject.Find("CardDescription");
        if (descriptionObject)
        {
            if (descriptionObject.GetComponent<Text>())
            {
                _descriptionText = descriptionObject.GetComponent<Text>();
            }
        }
        Ghost.transform.SetParent(World.Instance.transform);
        Ghost.transform.localScale = Vector3.one;
    }

    bool isActivating = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isSelected)
        {
            Card[] cards = FindObjectsOfType<Card>();
            foreach (Card card in cards)
            {
                card.Deselect();
            }
            Select();
        }
        else
        {
            if (GameController.Instance.IsAR)
            {
                ActivateCard();
            }
            isActivating = true;       
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameController.Instance.IsAR)
        {
            if (isActivating)
            {
                DeactivateCard();
                isActivating = false;               
            }        
        }
    }

    public void Select()
    {
        _isSelected = true;
        image.color = Color.white;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position+ _moveToOnSelect, _selectTime));
        Ghost.transform.localScale = World.Instance.transform.localScale;
        Ghost.SetActive(true);
        SetGameInfo();
        GameController.Instance.SetSelectedCard(this);
       
    }

    public void Deselect()
    {
        _isSelected = false;
        Ghost.SetActive(false);
        image.color = Color.gray;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position, _selectTime));
        GameInfo.Instance.SetText("");
    }

    protected virtual void SetGameInfo() {
        GameInfo.Instance.SetText(Description);
    }

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

        if (MyCursor.Instance.GetIsActionable())
        {
            if (_isSelected)
            {
                RaycastHit hit = MyCursor.Instance.GetCursorHit();
                UpdateGhost(hit);
            }
        }
        
        _rectTransform.localPosition = Vector3.MoveTowards(_rectTransform.localPosition, _targetPosition, 2000 * Time.deltaTime); ;
        
    }
    Vector3 _targetPosition;
    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    protected virtual void UpdateGhost(RaycastHit hit) {
        Ghost.transform.position = hit.point;
        Ghost.transform.up = Vector3.up;
    }

    public void Remove()
    {
        Destroy(Ghost);
        GameInfo.Instance.SetText("");
        if (Deck)
        {
            Deck.RemoveCard(this);
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
