using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Visual
    [SerializeField] private GameObject canvasPlayerCard;
    [SerializeField] private GameObject canvasIACard;
    [SerializeField] private Image iaCardSelected;
    [SerializeField] private Image playerCardSelected;
    [SerializeField] private GameObject playerDeckTable;
    [SerializeField] private GameObject iaDeckTable;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameButtons;
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private TMP_Text passText;
    [SerializeField] private TMP_Text aiUsedText;
    [SerializeField] private GameObject cloverDeckTable;
    [SerializeField] private GameObject diamondDeckTable;
    [SerializeField] private GameObject spadeDeckTable;
    [SerializeField] private GameObject heartDeckTable;
    [SerializeField] private GameObject backgroundCard;

    //Decks
    private bool turnPlayer;
    List<KeyValuePair<int, string>> deck = new List<KeyValuePair<int, string>>();
    List<KeyValuePair<int, string>> playerDeck = new List<KeyValuePair<int, string>>();
    List<KeyValuePair<int, string>> IADeck = new List<KeyValuePair<int, string>>();
    [SerializeField] private List<Sprite> spriteDeck;
    [SerializeField] private Sprite cardReverse;

    //Cards used in the Table
    private int beforeFiveClover = 0, afterFiveClover = 0;
    private int beforeFiveDiamond = 0, afterFiveDiamond = 0;
    private int beforeFiveSpade = 0, afterFiveSpade = 0;
    private int beforeFiveHeart = 0, afterFiveHeart = 0;
    private bool fiveClover = false, fiveDiamond = false, fiveSpade = false, fiveHeart = false;

    //Helper for Check Player Card
    private int numberCardPlayerSelected = 0;
    private string deckNamePlayerSelected = "";
    private string namePlayerCardUsed = "";

    private void Awake() 
    {
        passText.text = "";
        canvasPlayerCard.SetActive(false);
        canvasIACard.SetActive(false);
        FillDeck();
        ShuffeledCards();
        EmptyDecksTable();
        RandomStartGame();
    }
    
    private void Update() 
    {
        PlayerSelectCard();
        EndGame();
    }

    //Start Game
    private void FillDeck()
    {
        var suits = new string [] { "Clover", "Diamond", "Heart", "Spade" };

        var values = new List<int>() {
           1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13
        }; 

        foreach(var suit in suits)
        {
            foreach(var value in values)
            {
                deck.Add(new KeyValuePair<int, string>(value, suit));
            }
        }
    }
    
    private void ShuffeledCards()
    {
        int numberTotalCards = deck.Count;
        int numberPlayerCard = 1;

        for(int i = 1; numberTotalCards >= i; i++)
        {
            int numberCard = Random.Range(0, deck.Count);
            
            if(i % 2 == 0)
            {
                playerDeck.Add(deck[numberCard]);

                int numberDeckCard = deck[numberCard].Key;
                string nameDeckCard = deck[numberCard].Value;
                GameObject Player_Card = GameObject.Find("Player_Card_" + numberPlayerCard);

                int index = spriteDeck.FindIndex(s => s.name == numberDeckCard + "_" + nameDeckCard);

                Player_Card.GetComponent<SpriteRenderer>().sprite = spriteDeck[index];
                Player_Card.GetComponent<PlayerCard>().numberCard = deck[numberCard].Key;
                Player_Card.GetComponent<PlayerCard>().deckName = deck[numberCard].Value;
                numberPlayerCard++;
            } else {
                IADeck.Add(deck[numberCard]);
            }

            deck.RemoveAt(numberCard);            
        }
    }

    private void EmptyDecksTable()
    {
        //Deactive CloverCards
        for (int i = 0; i < cloverDeckTable.transform.childCount; i++)
        {
            var child = cloverDeckTable.transform.GetChild(i).gameObject;
            if (child != null) {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive DiamondCards
        for (int i = 0; i < diamondDeckTable.transform.childCount; i++)
        {
            var child = diamondDeckTable.transform.GetChild(i).gameObject;
            if (child != null) {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive SpadeCards
        for (int i = 0; i < spadeDeckTable.transform.childCount; i++)
        {
            var child = spadeDeckTable.transform.GetChild(i).gameObject;
            if (child != null) {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        //Deactive HeartCards
        for (int i = 0; i < heartDeckTable.transform.childCount; i++)
        {
            var child = heartDeckTable.transform.GetChild(i).gameObject;
            if (child != null) {
                child.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void RandomStartGame()
    {
        int randomNumber = Random.Range(0, 2);
   
        if(randomNumber == 1)
        {
            turnPlayer = true;
            canvasPlayerCard.SetActive(true);
            canvasIACard.SetActive(false);
        } else {
            turnPlayer = false;
            IATurn();
        }
    }

    //IA Functions
    private void IATurn()
    {
        canvasPlayerCard.SetActive(false);
        canvasIACard.SetActive(true);

        iaCardSelected.sprite = cardReverse;

        foreach(var cardIA in IADeck)
        {
            bool IA_use_card = false;
            int newBeforeFiveClover = 0;
            int newAfterFiveClover = 0;
            int newBeforeFiveDiamond = 0;
            int newAfterFiveDiamond = 0;
            int newBeforeFiveHeart = 0;
            int newAfterFiveHeart = 0;
            int newBeforeFiveSpade = 0;
            int newAfterFiveSpade = 0;

            if(cardIA.Value == "Clover")
            {
                if(!fiveClover) {
                    if(cardIA.Key == 5) 
                    {
                        var child = cloverDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        fiveClover = true;
                        beforeFiveClover = 5;
                        afterFiveClover = 5;
                        DestroyLastCardIA();
                        IA_use_card = true;
                    }
                } else {
                    if(cardIA.Key > 5)
                    {
                        newAfterFiveClover = afterFiveClover + 1;

                        if(cardIA.Key == newAfterFiveClover)
                        {
                            var child = cloverDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            afterFiveClover = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }

                    }
                    else if(cardIA.Key < 5)
                    {

                        newBeforeFiveClover = beforeFiveClover - 1;
                        if(cardIA.Key == newBeforeFiveClover)
                        {
                            var child = cloverDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            beforeFiveClover = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }
                    }
                }
            }

            if(cardIA.Value == "Diamond")
            {
                if(!fiveDiamond) {
                    if(cardIA.Key == 5) 
                    {
                        var child = diamondDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        fiveDiamond = true;
                        beforeFiveDiamond = 5;
                        afterFiveDiamond = 5;
                        DestroyLastCardIA();
                        IA_use_card = true;
                    }
                } else {
                    if(cardIA.Key > 5)
                    {
                        newAfterFiveDiamond = afterFiveDiamond + 1;
                        if(cardIA.Key == newAfterFiveDiamond)
                        {
                            var child = diamondDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            afterFiveDiamond = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }

                    }
                    else if(cardIA.Key < 5)
                    {

                        newBeforeFiveDiamond = beforeFiveDiamond - 1;
                        if(cardIA.Key == newBeforeFiveDiamond)
                        {
                            var child = diamondDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            beforeFiveDiamond = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }
                    }
                }
            }

            if(cardIA.Value == "Heart")
            {
                if(!fiveHeart) {
                    if(cardIA.Key == 5) 
                    {
                        var child = heartDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        fiveHeart = true;
                        beforeFiveHeart = 5;
                        afterFiveHeart = 5;
                        DestroyLastCardIA();
                        IA_use_card = true;
                    }
                } else {
                    if(cardIA.Key > 5)
                    {
                        newAfterFiveHeart = afterFiveHeart + 1;

                        if(cardIA.Key == newAfterFiveHeart)
                        {
                            var child = heartDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            afterFiveHeart = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }

                    }
                    else if(cardIA.Key < 5)
                    {

                        newBeforeFiveHeart = beforeFiveHeart - 1;
                        if(cardIA.Key == newBeforeFiveHeart)
                        {
                            var child = heartDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            beforeFiveHeart = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }
                    }
                }
            }

            if(cardIA.Value == "Spade")
            {
                if(!fiveSpade) {
                    if(cardIA.Key == 5) 
                    {
                        var child = spadeDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        fiveSpade = true;
                        beforeFiveSpade = 5;
                        afterFiveSpade = 5;
                        DestroyLastCardIA();
                        IA_use_card = true;
                    }
                } else {
                    if(cardIA.Key > 5)
                    {
                        newAfterFiveSpade = afterFiveSpade + 1;

                        if(cardIA.Key == newAfterFiveSpade)
                        {
                            var child = spadeDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            afterFiveSpade = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }

                    }
                    else if(cardIA.Key < 5)
                    {

                        newBeforeFiveSpade = beforeFiveSpade - 1;
                        if(cardIA.Key == newBeforeFiveSpade)
                        {
                            var child = spadeDeckTable.transform.GetChild(cardIA.Key - 1).gameObject;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            beforeFiveSpade = cardIA.Key;
                            DestroyLastCardIA();
                            IA_use_card = true;
                        }
                    }
                }
            }

            if(IA_use_card)
            {
                int index = spriteDeck.FindIndex(s => s.name == cardIA.Key + "_" + cardIA.Value);
                iaCardSelected.GetComponent<Image>().sprite = spriteDeck[index];
                aiUsedText.text = "AI used this card!";
                break;
            } else {
                aiUsedText.text = "AI can't use card";
            }

        }
    }

    private void DestroyLastCardIA()
    {
        for (var i = iaDeckTable.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(iaDeckTable.transform.GetChild(i).gameObject);
            break;
        }
    }
    
    public void StartPlayerTurn()
    {
        canvasPlayerCard.SetActive(true);
        canvasIACard.SetActive(false);
        turnPlayer = true;
    }

    //Player Functions
    public void PlayerTurnUseCard()
    {
        passText.text = "";
        playerCardSelected.sprite = cardReverse;

        int newBeforeFiveClover;
        int newAfterFiveClover;
        int newBeforeFiveDiamond;
        int newAfterFiveDiamond;
        int newBeforeFiveHeart;
        int newAfterFiveHeart;
        int newBeforeFiveSpade;
        int newAfterFiveSpade;
        bool playerUseCard = false;

        if(deckNamePlayerSelected == "Clover")
        {
            if(!fiveClover) {
                if(numberCardPlayerSelected == 5) 
                {
                    var child = cloverDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                    child.GetComponent<SpriteRenderer>().enabled = true;
                    fiveClover = true;
                    beforeFiveClover = 5;
                    afterFiveClover = 5;
                    playerUseCard = true;
                }
            } else {
                if(numberCardPlayerSelected > 5)
                {
                    newAfterFiveClover = afterFiveClover + 1;

                    if(numberCardPlayerSelected == newAfterFiveClover)
                    {
                        var child = cloverDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        afterFiveClover = numberCardPlayerSelected;
                        playerUseCard = true;
                    }

                }
                else if(numberCardPlayerSelected < 5)
                {

                    newBeforeFiveClover = beforeFiveClover - 1;
                    if(numberCardPlayerSelected == newBeforeFiveClover)
                    {
                        var child = cloverDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        beforeFiveClover = numberCardPlayerSelected;
                        playerUseCard = true;
                    }
                }
            }
        }

        if(deckNamePlayerSelected == "Diamond")
        {
            if(!fiveDiamond) {
                if(numberCardPlayerSelected == 5) 
                {
                    var child = diamondDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                    child.GetComponent<SpriteRenderer>().enabled = true;
                    fiveDiamond = true;
                    beforeFiveDiamond = 5;
                    afterFiveDiamond = 5;
                    playerUseCard = true;
                }
            } else {
                if(numberCardPlayerSelected > 5)
                {
                    newAfterFiveDiamond = afterFiveDiamond + 1;
                    if(numberCardPlayerSelected == newAfterFiveDiamond)
                    {
                        var child = diamondDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        afterFiveDiamond = numberCardPlayerSelected;
                        playerUseCard = true;
                    }

                }
                else if(numberCardPlayerSelected < 5)
                {

                    newBeforeFiveDiamond = beforeFiveDiamond - 1;
                    if(numberCardPlayerSelected == newBeforeFiveDiamond)
                    {
                        var child = diamondDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        beforeFiveDiamond = numberCardPlayerSelected;
                        playerUseCard = true;
                    }
                }
            }
        }

        if(deckNamePlayerSelected == "Heart")
        {
            if(!fiveHeart) {
                if(numberCardPlayerSelected == 5) 
                {
                    var child = heartDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                    child.GetComponent<SpriteRenderer>().enabled = true;
                    fiveHeart = true;
                    beforeFiveHeart = 5;
                    afterFiveHeart = 5;
                    playerUseCard = true;
                }
            } else {
                if(numberCardPlayerSelected > 5)
                {
                    newAfterFiveHeart = afterFiveHeart + 1;
                    if(numberCardPlayerSelected == newAfterFiveHeart)
                    {
                        var child = heartDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        afterFiveHeart = numberCardPlayerSelected;
                        playerUseCard = true;
                    }

                }
                else if(numberCardPlayerSelected < 5)
                {

                    newBeforeFiveHeart = beforeFiveHeart - 1;
                    if(numberCardPlayerSelected == newBeforeFiveHeart)
                    {
                        var child = heartDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        beforeFiveHeart = numberCardPlayerSelected;
                        playerUseCard = true;
                    }
                }
            }
        }

        if(deckNamePlayerSelected == "Spade")
        {
            if(!fiveSpade) {
                if(numberCardPlayerSelected == 5) 
                {
                    var child = spadeDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                    child.GetComponent<SpriteRenderer>().enabled = true;
                    fiveSpade = true;
                    beforeFiveSpade = 5;
                    afterFiveSpade = 5;
                    playerUseCard = true;
                }
            } else {
                if(numberCardPlayerSelected > 5)
                {
                    newAfterFiveSpade = afterFiveSpade + 1;

                    if(numberCardPlayerSelected == newAfterFiveSpade)
                    {
                        var child = spadeDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        afterFiveSpade = numberCardPlayerSelected;
                        playerUseCard = true;
                    }

                }
                else if(numberCardPlayerSelected < 5)
                {

                    newBeforeFiveSpade = beforeFiveSpade - 1;
                    if(numberCardPlayerSelected == newBeforeFiveSpade)
                    {
                        var child = spadeDeckTable.transform.GetChild(numberCardPlayerSelected - 1).gameObject;
                        child.GetComponent<SpriteRenderer>().enabled = true;
                        beforeFiveSpade = numberCardPlayerSelected;
                        playerUseCard = true;
                    }
                }
            }
        }

        if(playerUseCard)
        {
            GameObject Player_Card = GameObject.Find(namePlayerCardUsed);
            Destroy(Player_Card);
            IATurn();
            turnPlayer = false;
        }

    }

    private void PlayerSelectCard()
    {
        if(turnPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray, 1500f);

                if(hit.collider != null)
                {

                    GameObject cardSelected = hit.collider.gameObject;
                    numberCardPlayerSelected = cardSelected.GetComponent<PlayerCard>().numberCard;
                    deckNamePlayerSelected = cardSelected.GetComponent<PlayerCard>().deckName;

                    int index = spriteDeck.FindIndex(s => s.name == numberCardPlayerSelected + "_" + deckNamePlayerSelected);
                    namePlayerCardUsed = hit.collider.name;
                    playerCardSelected.GetComponent<Image>().sprite = spriteDeck[index];

                    //Background
                    Destroy(GameObject.Find("BackgroundCard(Clone)"));
                    GameObject playerDeck = GameObject.Find(hit.collider.name);
                    GameObject myBackgroundCard = Instantiate(backgroundCard);
                    myBackgroundCard.transform.parent = playerDeck.transform;
                    myBackgroundCard.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }

    public void PlayerPassTurn()
    {
        if(!PlayerCheckCards()) {
            passText.text = "You can't pass because you have cards";
        } 
        else 
        {
            passText.text = "";
            canvasPlayerCard.SetActive(false);
            canvasIACard.SetActive(true);
            turnPlayer = false;
            IATurn();
        }
    }

    private bool PlayerCheckCards()
    {

        int newBeforeFiveClover = 0;
        int newAfterFiveClover = 0;
        int newBeforeFiveDiamond = 0;
        int newAfterFiveDiamond = 0;
        int newBeforeFiveHeart = 0;
        int newAfterFiveHeart = 0;
        int newBeforeFiveSpade = 0;
        int newAfterFiveSpade = 0;

        foreach(var playerCard in playerDeck)
        {
                
            if(playerCard.Value == "Clover")
            {
                if(!fiveClover) {
                    if(playerCard.Key == 5) 
                    {
                        return false;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveClover = afterFiveClover + 1;

                        if(playerCard.Key == newAfterFiveClover)
                        {
                            return false;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveClover = beforeFiveClover - 1;
                        if(playerCard.Key == newBeforeFiveClover)
                        {
                            return false;
                        }
                    }
                }
            }

            if(playerCard.Value == "Diamond")
            {
                if(!fiveDiamond) {
                    if(playerCard.Key == 5) 
                    {
                        return false;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveDiamond = afterFiveDiamond + 1;
                        if(playerCard.Key == newAfterFiveDiamond)
                        {
                            return false;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveDiamond = beforeFiveDiamond - 1;
                        if(playerCard.Key == newBeforeFiveDiamond)
                        {
                            return false;
                        }
                    }
                }
            }

            if(playerCard.Value == "Heart")
            {
                if(!fiveHeart) {
                    if(playerCard.Key == 5) 
                    {
                        return false;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveHeart = afterFiveHeart + 1;

                        if(playerCard.Key == newAfterFiveHeart)
                        {
                            return false;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveHeart = beforeFiveHeart - 1;
                        if(playerCard.Key == newBeforeFiveHeart)
                        {
                            return false;
                        }
                    }
                }
            }

            if(playerCard.Value == "Spade")
            {
                if(!fiveSpade) {
                    if(playerCard.Key == 5) 
                    {
                        return false;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveSpade = afterFiveSpade + 1;

                        if(playerCard.Key == newAfterFiveSpade)
                        {
                            return false;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveSpade = beforeFiveSpade - 1;
                        if(playerCard.Key == newBeforeFiveSpade)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    //EndGame
    private void EndGame()
    {
        bool endGame = false;

        if(playerDeckTable.transform.childCount == 0) {
            endGameText.text = "You Win! :)";
            endGame = true;
        } else if (iaDeckTable.transform.childCount == 0) {
            endGameText.text = "You Lost :(";
            endGame = true;
        }

        if(endGame)
        {
            gameButtons.SetActive(false);
            iaDeckTable.SetActive(false);
            victoryPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

}
