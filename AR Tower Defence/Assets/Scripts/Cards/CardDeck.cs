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
    [SerializeField] float _dealTime = 0.3f;
    [SerializeField] RectTransform _hand;
    List<Card> _cardsInHand = new List<Card>();


    private void Start()
    {
          
    }

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
        if (Points.instance.PurchaseCardDraw())
        {
            if (_DrawAllTypes)
            {
                for (int i = 0; i < _cardPrefabs.Count; i++)
                {
                    Card card = Instantiate(_cardPrefabs[i],transform);
                    card.Deck = this;
                    _cardsInHand.Add(card);
                    StartCoroutine(PositionCard(card, _cardsInHand.Count-1, _dealTime));
                }
            }
        }
    }

    IEnumerator PositionCard(Card card, int index, float duration) {
        Vector3 start = card.GetComponent<RectTransform>().position;
        card.GetComponent<RectTransform>().position = start;
        yield return new WaitForSeconds(index * duration/2);

        card.gameObject.SetActive(true);
        Vector3 end = _hand.position - index*(card.GetComponent<RectTransform>().rect.width + 10) * Vector3.right;
        for (float i = 0; i < duration; i += 0.01f) {

            card.GetComponent<RectTransform>().position = Vector3.Lerp(start,end,i/duration);

            yield return new WaitForSeconds(0.01f);
        }
        card.GetComponent<RectTransform>().position = end;
    }

    public void OnActivateCard(Card card) {
        _cardsInHand.Remove(card);
        Destroy(card.gameObject);
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            StartCoroutine(PositionCard(_cardsInHand[i], i, 0.1f));
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
