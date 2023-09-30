using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuffeledCards : MonoBehaviour
{
    //Singleton
    public static ShuffeledCards Instance;

    //Deck
    private List<KeyValuePair<int, string>> deck = new List<KeyValuePair<int, string>>();

    //UI Cards Table
    [SerializeField] private GameObject cloverDeckTable;
    [SerializeField] private GameObject diamondDeckTable;
    [SerializeField] private GameObject spadeDeckTable;
    [SerializeField] private GameObject heartDeckTable;
    [SerializeField] private GameObject startPointHumanCards;
    [SerializeField] private GameObject AICards;
    [SerializeField] private GameObject backgroundCardIA;
    [SerializeField] private Sprite spriteBackgroundCardIA;


    private void Awake()
    {
        Instance = this;
        FillDeck();
        EmptyDecksTable();
    }

    private void Start()
    {
        ShuffeledGameCards();
    }

    private void FillDeck()
    {
        var suits = new string[] { "Clover", "Diamond", "Heart", "Spade" };

        var values = new List<int>() {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13
        };

        foreach (var suit in suits)
        {
            foreach (var value in values)
            {
                deck.Add(new KeyValuePair<int, string>(value, suit));
            }
        }
    }

    private void EmptyDecksTable()
    {
        //Deactive CloverCards
        for (int i = 0; i < cloverDeckTable.transform.childCount; i++)
        {
            var child = cloverDeckTable.transform.GetChild(i).gameObject;
            if (child != null)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive DiamondCards
        for (int i = 0; i < diamondDeckTable.transform.childCount; i++)
        {
            var child = diamondDeckTable.transform.GetChild(i).gameObject;
            if (child != null)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive SpadeCards
        for (int i = 0; i < spadeDeckTable.transform.childCount; i++)
        {
            var child = spadeDeckTable.transform.GetChild(i).gameObject;
            if (child != null)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive HeartCards
        for (int i = 0; i < heartDeckTable.transform.childCount; i++)
        {
            var child = heartDeckTable.transform.GetChild(i).gameObject;
            if (child != null)
            {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void ShuffeledGameCards()
    {
        int numberPlayers = GameManager.Instance.numberPlayers;
        int numberTotalCards = deck.Count;
        int playerShuffeled = 1;


        for (int i = 1; numberTotalCards >= i; i++)
        {
            //Restart Player Shuffeled
            if (playerShuffeled > numberPlayers) { playerShuffeled = 1; }

            int playerShuffeledList =  playerShuffeled - 1;
            GameObject player = GameManager.Instance.playersDeck[playerShuffeledList];

            int numberCard = Random.Range(0, deck.Count);
            int numberDeckCard = deck[numberCard].Key;
            string nameDeckCard = deck[numberCard].Value;

            player.GetComponent<PlayerCard>().playerDeck.Add(new KeyValuePair<int, string>(numberDeckCard, nameDeckCard));

            //NextPlayer
            playerShuffeled++;

            //Remove from Deck
            deck.RemoveAt(numberCard);
        }

        if(GameManager.Instance.humanPlay)
        {
            GameObject HumanPlayer = GameObject.Find("playerNumber1");

            var playerDeck = HumanPlayer.GetComponent<PlayerCard>().playerDeck;
            int numberCards = playerDeck.Count;
            float positionValue = 16f / (float)numberCards;
            float actualposition = -7.7f;

            for (int i = 0; numberCards > i; i++)
            {
                GameObject humanCardNumber = new GameObject();
                humanCardNumber.AddComponent<HumanCardTable>();

                humanCardNumber.GetComponent<HumanCardTable>().numberCard = playerDeck[i].Key;
                humanCardNumber.GetComponent<HumanCardTable>().deckName = playerDeck[i].Value;

                int index = GameManager.Instance.spriteDeck.FindIndex(s => s.name == playerDeck[i].Key + "_" + playerDeck[i].Value);

                humanCardNumber.AddComponent<BoxCollider2D>();
                humanCardNumber.AddComponent<SpriteRenderer>();
                humanCardNumber.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.spriteDeck[index];
                humanCardNumber.GetComponent<SpriteRenderer>().sortingOrder = i+1;

                humanCardNumber.transform.SetParent(startPointHumanCards.transform);

                humanCardNumber.name = "humanCardNumber" + (i+1);

                humanCardNumber.transform.position = new Vector3(actualposition, -4, 0);
                actualposition = actualposition + positionValue;
            }
        }

        ShowDeckIA();
    }

    private void ShowDeckIA()
    {
        int numberIA = GameManager.Instance.numberIA;

        float positionValue = 16f / (float)numberIA;
        float actualposition = -7.7f;

        for(int i = 1; numberIA >= i; i++)
        {
            GameObject AIDeck = new GameObject();
            AIDeck.AddComponent<SpriteRenderer>();
            AIDeck.GetComponent<SpriteRenderer>().sprite = spriteBackgroundCardIA;

            // AICards
            AIDeck.transform.SetParent(AICards.transform);
            AIDeck.name = "AICards" + i;
            if(numberIA == 1)
            {
                AIDeck.transform.position = new Vector3(0, 4, 0);
            } else {
                AIDeck.transform.position = new Vector3(actualposition, 4, 0);
                actualposition = actualposition + positionValue;
            }

        }
    }

}
