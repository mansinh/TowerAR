using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image _image;
    [SerializeField] GameObject _ghost;

    CardDeck deck;

    Vector3 _originalImagePosition;
    RectTransform _rectTransform;

    bool _isSelected = false;

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
            Deselect();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void Select()
    {
        _isSelected = true;
        _image.color = Color.white;
        _ghost.SetActive(true);
        if (GameController.instance.IsAR)
        {

        }
    }

    public void Deselect()
    {
        _isSelected = false;
        _ghost.SetActive(false);
        _image.color = Color.gray;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image.color = Color.gray;
        _ghost.transform.parent = WorldRoot.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (MyCursor.instance.GetCursorHitting())
        {
            if (_isSelected)
            {
                RaycastHit hit = MyCursor.instance.GetCursorHit();
                UpdateGhost(hit);

                if (GameController.instance.IsAR)
                {

                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        ApplyCard(hit);
                        Deselect();
                    }
                }
            }
        }
    }

    protected virtual void UpdateGhost(RaycastHit hit) {
        _ghost.transform.position = hit.transform.position;
    }

    protected virtual void ApplyCard(RaycastHit hit)
    {
        
    }
}
