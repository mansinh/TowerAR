using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image _image;
    [SerializeField] protected GameObject Ghost;
    [SerializeField] float _selectTime = 0.07f;
    [SerializeField] protected string Description = "Basic Card";

    public CardDeck Deck;
    Text _descriptionText;
    RectTransform _rectTransform;

    bool _isSelected = false;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image.color = Color.gray;
        GameObject descriptionObject = GameObject.Find("CardDescription");
        if (descriptionObject)
        {
            if (descriptionObject.GetComponent<Text>())
            {
                _descriptionText = descriptionObject.GetComponent<Text>();
            }
        }
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
        _image.color = Color.white;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position+ Vector3.up * 20, _selectTime));
        Ghost.SetActive(true);
        GameInfo.Instance.SetText(Description);
    }

    public void Deselect()
    {
        _isSelected = false;
        Ghost.SetActive(false);
        _image.color = Color.gray;
        StartCoroutine(MoveCard(GetComponent<RectTransform>().position, _selectTime));
        GameInfo.Instance.SetText("");
    }

    IEnumerator MoveCard(Vector3 moveTo, float duration)
    {
        Vector3 moveFrom = _image.GetComponent<RectTransform>().position;     
        for (float i = 0; i < duration; i += 0.01f)
        {
            _image.GetComponent<RectTransform>().position = Vector3.Lerp(moveFrom, moveTo, i / duration);

            yield return new WaitForSeconds(0.01f);
        }
        _image.GetComponent<RectTransform>().position = moveTo;
    }

    void Update()
    {
        if (MyCursor.Instance.GetCursorHitting())
        {
            if (_isSelected)
            {
                RaycastHit hit = MyCursor.Instance.GetCursorHit();
                UpdateGhost(hit);
                if (!GameController.Instance.IsAR)
                {               
                    if (Input.GetMouseButton(0))
                    {
                        ActivateCard();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        Deselect();
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        DeactivateCard();
                    }
                }
            }
        }
    }

    protected virtual void UpdateGhost(RaycastHit hit) {
        Ghost.transform.position = hit.point;
        Ghost.transform.up = Vector3.up;
    }

    protected virtual void ActivateCard(){}
    protected virtual void DeactivateCard(){}
}
