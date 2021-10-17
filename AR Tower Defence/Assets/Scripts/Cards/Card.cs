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
            isActivating = true;       
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActivating = false;
    }

    public void Select()
    {
        _isSelected = true;
        _image.color = Color.white;
        StartCoroutine(MoveImage(GetComponent<RectTransform>().position+ Vector3.up * 20, _selectTime));
        Ghost.SetActive(true);
        GameInfo.Instance.SetText(Description);
    }

    public void Deselect()
    {
        _isSelected = false;
        Ghost.SetActive(false);
        _image.color = Color.gray;
        StartCoroutine(MoveImage(GetComponent<RectTransform>().position, _selectTime));
        GameInfo.Instance.SetText("");
    }

    IEnumerator MoveImage(Vector3 moveTo, float duration)
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
        if (MyCursor.instance.GetCursorHitting())
        {
            if (_isSelected)
            {
                RaycastHit hit = MyCursor.instance.GetCursorHit();
                UpdateGhost(hit);
                if (GameController.Instance.IsAR)
                {
                    if (isActivating)
                    {
                        ActivateCard();
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        ActivateCard();
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        Deselect();
                    }
                }
            }
        }
    }

    protected virtual void UpdateGhost(RaycastHit hit) {
        Ghost.transform.position = hit.point;
        Ghost.transform.up = Vector3.up;
    }

    protected virtual void ActivateCard()
    {
       
    }
}
