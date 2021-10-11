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

    [SerializeField] bool _isTesting;
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

    public void DealCards() {
        if (_isTesting) {
            for(int i = 0; i < _cardPrefabs.Count;i++) {
                Card card = Instantiate(_cardPrefabs[i], _hand);
                
                _cardsInHand.Add(card);

            }
        }
    }

    IEnumerator MoveCardToHand(Card card) {
        for (float i = 0; i < _dealTime; i += 0.05f)
        {
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("deck up");
        DealCards();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("deck down");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("deck click");
    }

}
