using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class CardDeck : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,IPointerUpHandler
{
    [SerializeField] List<Card> _cardPrefabs;
    [SerializeField] List<float> _cardProbability;
    [SerializeField] bool _DrawAllTypes;
    [SerializeField] int _dealSize = 5;
    [SerializeField] float _cardSpacing = 10;
    [SerializeField] int _maxCards = 15;
    [SerializeField] bool _isCentered = false;
    [SerializeField] RectTransform _hand;
    [SerializeField] string _deckType = "Main";
    List<Card> _cardsInHand = new List<Card>();

    private void Update()
    {
        if (_cardPrefabs.Count > _cardProbability.Count) {
            _cardProbability.Add(0);
        }
        else if (_cardPrefabs.Count < _cardProbability.Count) {
            _cardProbability.Remove(_cardProbability.Count-1);
        }
    }

    public void DrawCards() {
        if (_cardsInHand.Count <= _maxCards)
        { 
            if (Points.Instance.PurchaseCardDraw(_deckType))
            {
                if (_DrawAllTypes)
                {
                    int dealSize = Mathf.Min(_maxCards - _cardsInHand.Count, _cardPrefabs.Count);
                    for (int i = 0; i < _cardPrefabs.Count; i++)
                    {
                        Card card = Instantiate(_cardPrefabs[i], transform);
                        card.Deck = this;
                        _cardsInHand.Add(card);                     
                    }
                }
                else
                {
                    int dealSize = Mathf.Min(_maxCards - _cardsInHand.Count, _dealSize);
                    for (int i = 0; i < dealSize; i++)
                    {
                        DrawRandomCard();
                    }                 
                }
                UpdateCardPositions();
            }
        }
    }

    protected virtual Card DrawRandomCard() {
        int cardType = (int)(_cardPrefabs.Count * Random.value);
        if (GameObject.Find("Tower(Clone)") == null && _cardsInHand.Capacity <= 0)
        {
            while (cardType != 0)
            {
                cardType = (int)(_cardPrefabs.Count * Random.value);
            }
        }
        Card card = Instantiate(_cardPrefabs[cardType], transform);
        card.Deck = this;
        _cardsInHand.Add(card);
        return card;
    }

    public void RemoveCard(Card card)
    {
        _cardsInHand.Remove(card);
        Destroy(card.gameObject);
        UpdateCardPositions();
    }

    Vector3 GetCardPosition(int index) {
        Vector3 position = _hand.position - index * _cardSpacing * Vector3.right;
        if (_isCentered)
        {
            position += _cardsInHand.Count * _cardSpacing * Vector3.right / 2;
        }
        return position;
    }

    public void UpdateCardPositions()
    {
        
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            _cardsInHand[i].SetTargetPosition(GetCardPosition(i));
            
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {  
        DrawCards();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //print("");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("");
    }
}
