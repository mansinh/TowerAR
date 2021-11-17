using System.Collections;
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

[ExecuteAlways]
public class CardDeck : MonoBehaviour, IPointerClickHandler
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
    [SerializeField] private List<int> cardFrequencies;
    [SerializeField] private List<Card> probabilities;

    [SerializeField] private bool drawAllTypes; //Used for testing cards
    [SerializeField] private int dealSize = 3;
    [SerializeField] private float cardSpacing = 10;
    [SerializeField] private int maxCards = 15;
    [SerializeField] private DeckType deckType = DeckType.Main;
    private List<Card> _cardsInHand = new List<Card>();
    private float _cardWidth;

    private void Start()
    {
        CalculateProbabilities();
    }

    void CalculateProbabilities()
    {
        probabilities = new List<Card>();
        for (int j = 0; j < cardFrequencies.Count;j++)
        {
            int frequency = cardFrequencies[j];
            for (int i = 0; i < frequency; i++)
            {
                probabilities.Add(cardPrefabs[j]);
            }
        }
    }

    private void Update()
    {
        if (cardPrefabs.Count > cardFrequencies.Count) {
            cardFrequencies.Add(0);
        }
        else if (cardPrefabs.Count < cardFrequencies.Count) {
            cardFrequencies.Remove(cardFrequencies.Count-1);
        }
    }

    public bool CanDrawCard()
    {
        return _cardsInHand.Count < maxCards;
    }

    public void DrawCards() {
        if (CanDrawCard())
        { 
            //Purchase card draw if player has enough points
            if (Points.Instance.PurchaseCardDraw(deckType))
            {
                //For testing purposes, draw one of each of the different types of cards
                if (drawAllTypes)
                {
                    DrawAll();
                }
                else
                {
                    StartCoroutine(DrawRandom());
                }
                UpdateCardPositions();
            }
        }
    }

    public void DrawLumberCards()
    {
        if (CanDrawCard())
        {
            if (drawAllTypes)
            {
                DrawAll();
            }
            else
            {
                StartCoroutine(DrawRandom());
            }
            UpdateCardPositions();
        }   
    }

    void DrawAll()
    {
        for (int i = 0; i < cardPrefabs.Count; i++)
        {
            Card card = Instantiate(cardPrefabs[i], transform);
            card.Deck = this;
            _cardsInHand.Add(card);
        }
    }

    IEnumerator DrawRandom()
    {
        //Draw a random set of cards
        int dealSize = Mathf.Min(maxCards - _cardsInHand.Count, this.dealSize);
        for (int i = 0; i < dealSize; i++)
        {
            DrawRandomCard();
            SoundManager.Instance.Play(SoundManager.SoundType.DealCard);
            yield return new WaitForSeconds(0.1f);
            
        }
    }

    protected virtual Card DrawRandomCard() {
        int random = (int)(probabilities.Count * Random.value);

        Card card = Instantiate(probabilities[random], transform);
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

    [SerializeField] bool canDrawOnClick = true;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (canDrawOnClick)
        {
            DrawCards();
        }
    }
}
