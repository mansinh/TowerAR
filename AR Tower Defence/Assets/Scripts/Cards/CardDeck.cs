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
    [SerializeField] float _dealTime = 0.2f;
    [SerializeField] float _dealSize = 5f;
    [SerializeField] float _cardSpacing = 10;
    [SerializeField] int _maxCards = 10;
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
        if (_cardsInHand.Count <= _maxCards-_dealSize)
        { 
            if (Points.instance.PurchaseCardDraw(_deckType))
            {
                if (_DrawAllTypes)
                {
                    for (int i = 0; i < _cardPrefabs.Count; i++)
                    {
                        Card card = Instantiate(_cardPrefabs[i], transform);
                        card.Deck = this;
                        _cardsInHand.Add(card);                     
                    }
                }
                else
                {
                    for (int i = 0; i < _dealSize; i++)
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

    IEnumerator PositionCard(Card card, int index, float duration) {
        Vector3 start = card.GetComponent<RectTransform>().position;
        card.GetComponent<RectTransform>().position = start;
        yield return new WaitForSeconds(index * duration/4);

        card.gameObject.SetActive(true);
        Vector3 end = _hand.position - index* _cardSpacing * Vector3.right;

        if (_isCentered)
        {
            end += _cardsInHand.Count * _cardSpacing * Vector3.right/2;
        }

        for (float i = 0; i < duration; i += 0.01f) {

            card.GetComponent<RectTransform>().position = Vector3.Lerp(start,end,i/duration);
            yield return new WaitForSeconds(0.01f);
        }
        card.GetComponent<RectTransform>().position = end;
    }

    public void UpdateCardPositions()
    {
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            StartCoroutine(PositionCard(_cardsInHand[i], i, _dealTime));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {  
        DrawCards();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("");
    }
}
