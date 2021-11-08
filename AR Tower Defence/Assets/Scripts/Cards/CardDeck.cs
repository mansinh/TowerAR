using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Tap to purchase action cards and draw into hand
 * Draws are animated and cards reposition themselves in hand depending on number of cards
 * TODO: make different card types have different draw probablities and balance
 * @author Manny Kwong
 * @author Matthew Bogdanov
 */

public class CardDeck : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,IPointerUpHandler
{
    public enum DeckType
    {
        Main,
        Lightning,
        Water,
        Fire,
        Heal
    }

    [SerializeField] private List<Card> cardPrefabs;
    [SerializeField] private List<float> cardProbability;
    [SerializeField] private bool drawAllTypes; //Used for testing cards
    [SerializeField] private int dealSize = 5;
    [SerializeField] private float cardSpacing = 10;
    [SerializeField] private int maxCards = 15;
    [SerializeField] private DeckType deckType = DeckType.Main;
    private List<Card> _cardsInHand = new List<Card>();
    private float _cardWidth;

    private void Update()
    {
        if (cardPrefabs.Count > cardProbability.Count) {
            cardProbability.Add(0);
        }
        else if (cardPrefabs.Count < cardProbability.Count) {
            cardProbability.Remove(cardProbability.Count-1);
        }
    }

    public void DrawCards() {
        if (_cardsInHand.Count < maxCards)
        { 
            //Purchase card draw if player has enough points
            if (Points.Instance.PurchaseCardDraw(deckType))
            {
                //For testing purposes, draw one of each of the different types of cards
                if (drawAllTypes)
                {
                    //checks if there's tower on field/in the hand and if there's none it gives player a guaranteed tower card
                    int dealSize = Mathf.Min(maxCards - _cardsInHand.Count, cardPrefabs.Count);
                    for (int i = 0; i < cardPrefabs.Count; i++)
                    {
                        Card card = Instantiate(cardPrefabs[i], transform);
                        card.Deck = this;
                        _cardsInHand.Add(card);                     
                    }
                }
                else
                {
                    //Draw a random set of cards
                    int dealSize = Mathf.Min(maxCards - _cardsInHand.Count, this.dealSize);
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
        int cardType = (int)(cardPrefabs.Count * Random.value);

        //Matthew Bogdanov, draw a tower if none is in hand
        if (GameObject.Find("Tower(Clone)") == null && _cardsInHand.Capacity <= 0)
        {
            while (cardType != 0)
            {
                cardType = (int)(cardPrefabs.Count * Random.value);
            }
        }
        Card card = Instantiate(cardPrefabs[cardType], transform);
        card.Deck = this;
        _cardsInHand.Add(card);
        return card;
    }

    //Destroy card and update card hand positions when card is discarded
    public void DiscardCard(Card card)
    {
        _cardsInHand.Remove(card);
        Destroy(card.gameObject,0.1f);
        UpdateCardPositions();
    }

    //Space cards out over hand
    Vector3 GetCardPosition(int index) {
        Vector3 position = (index+1) * cardSpacing * Vector3.right;     
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
