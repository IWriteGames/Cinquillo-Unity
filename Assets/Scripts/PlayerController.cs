using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    //Cards used in the Table
    private int beforeFiveClover = 0, afterFiveClover = 0;
    private int beforeFiveDiamond = 0, afterFiveDiamond = 0;
    private int beforeFiveSpade = 0, afterFiveSpade = 0;
    private int beforeFiveHeart = 0, afterFiveHeart = 0;
    private bool fiveClover = false, fiveDiamond = false, fiveSpade = false, fiveHeart = false;

    [SerializeField] private AudioClip moveCard;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerTurn()
    {
        GameManager.Instance.ChangePlayerTurn();
        int playerTurn = GameManager.Instance.playerTurn;
        if(GameManager.Instance.HumanPlayerTurn())
        {
            GameManager.Instance.InfoText.text = "Your Turn";
        } else {
            GameManager.Instance.InfoText.text = "It is the turn of player number " + playerTurn;
        }

        if(GameManager.Instance.humanPlay && playerTurn == 1)
        {
            //NOTHING
        } else {
            if(PlayerCheckCards(playerTurn))
            {
                IASelectCard(playerTurn);
            } else {
                PlayerTurn();
            }
        }
    }

    private void IASelectCard(int playerTurn)
    {

        List<KeyValuePair<int, string>> cardSelected = new List<KeyValuePair<int, string>>();

        GameObject Player = GameObject.Find("playerNumber"+playerTurn);

        var playerDeck = Player.GetComponent<PlayerCard>().playerDeck;

        foreach(var playerCard in playerDeck)
        {
            int newBeforeFiveClover;
            int newAfterFiveClover;
            int newBeforeFiveDiamond;
            int newAfterFiveDiamond;
            int newBeforeFiveHeart;
            int newAfterFiveHeart;
            int newBeforeFiveSpade;
            int newAfterFiveSpade;

            if(playerCard.Value == "Clover")
            {
                if(!fiveClover) {
                    if(playerCard.Key == 5) 
                    {
                        fiveClover = true;
                        beforeFiveClover = 5;
                        afterFiveClover = 5;
                        cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                        break;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveClover = afterFiveClover + 1;

                        if(playerCard.Key == newAfterFiveClover)
                        {
                            afterFiveClover = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }
                    }
                    else if(playerCard.Key < 5)
                    {
                        newBeforeFiveClover = beforeFiveClover - 1;

                        if(playerCard.Key == newBeforeFiveClover)
                        {
                            beforeFiveClover = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }
                    }
                }
            }

            if(playerCard.Value == "Diamond")
            {
                if(!fiveDiamond) {
                    if(playerCard.Key == 5) 
                    {
                        fiveDiamond = true;
                        beforeFiveDiamond = 5;
                        afterFiveDiamond = 5;
                        cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                        break;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveDiamond = afterFiveDiamond + 1;
                        if(playerCard.Key == newAfterFiveDiamond)
                        {
                            afterFiveDiamond = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {
                        newBeforeFiveDiamond = beforeFiveDiamond - 1;
                        if(playerCard.Key == newBeforeFiveDiamond)
                        {
                            beforeFiveDiamond = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }
                    }
                }
            }

            if(playerCard.Value == "Heart")
            {
                if(!fiveHeart) {
                    if(playerCard.Key == 5) 
                    {
                        fiveHeart = true;
                        beforeFiveHeart = 5;
                        afterFiveHeart = 5;
                        cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                        break;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveHeart = afterFiveHeart + 1;

                        if(playerCard.Key == newAfterFiveHeart)
                        {
                            afterFiveHeart = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {
                        newBeforeFiveHeart = beforeFiveHeart - 1;
                        if(playerCard.Key == newBeforeFiveHeart)
                        {
                            beforeFiveHeart = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }
                    }
                }
            }

            if(playerCard.Value == "Spade")
            {
                if(!fiveSpade) {
                    if(playerCard.Key == 5) 
                    {
                        fiveSpade = true;
                        beforeFiveSpade = 5;
                        afterFiveSpade = 5;
                        cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                        break;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveSpade = afterFiveSpade + 1;

                        if(playerCard.Key == newAfterFiveSpade)
                        {
                            afterFiveSpade = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {
                        newBeforeFiveSpade = beforeFiveSpade - 1;
                        if(playerCard.Key == newBeforeFiveSpade)
                        {
                            beforeFiveSpade = playerCard.Key;
                            cardSelected.Add(new KeyValuePair<int, string>(playerCard.Key, playerCard.Value));
                            break;
                        }
                    }
                }
            }
        }

        StartCoroutine(MoveCard(
            cardSelected
        ));
    }

    public void HumanUpdateValuesTable(List<KeyValuePair<int, string>> cardSelected)
    {
        int numberCard = cardSelected[0].Key;
        string deckCard = cardSelected[0].Value;

        int newBeforeFiveClover;
        int newAfterFiveClover;
        int newBeforeFiveDiamond;
        int newAfterFiveDiamond;
        int newBeforeFiveHeart;
        int newAfterFiveHeart;
        int newBeforeFiveSpade;
        int newAfterFiveSpade;

        if(deckCard == "Clover")
        {
            if(!fiveClover) {
                if(numberCard == 5) 
                {
                    fiveClover = true;
                    beforeFiveClover = 5;
                    afterFiveClover = 5;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveClover = afterFiveClover + 1;

                    if(numberCard == newAfterFiveClover)
                    {
                        afterFiveClover = numberCard;
                    }
                }
                else if(numberCard < 5)
                {
                    newBeforeFiveClover = beforeFiveClover - 1;

                    if(numberCard == newBeforeFiveClover)
                    {
                        beforeFiveClover = numberCard;
                    }
                }
            }
        }

        if(deckCard == "Diamond")
        {
            if(!fiveDiamond) {
                if(numberCard == 5) 
                {
                    fiveDiamond = true;
                    beforeFiveDiamond = 5;
                    afterFiveDiamond = 5;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveDiamond = afterFiveDiamond + 1;
                    if(numberCard == newAfterFiveDiamond)
                    {
                        afterFiveDiamond = numberCard;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveDiamond = beforeFiveDiamond - 1;
                    if(numberCard == newBeforeFiveDiamond)
                    {
                        beforeFiveDiamond = numberCard;
                    }
                }
            }
        }

        if(deckCard == "Heart")
        {
            if(!fiveHeart) {
                if(numberCard == 5) 
                {
                    fiveHeart = true;
                    beforeFiveHeart = 5;
                    afterFiveHeart = 5;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveHeart = afterFiveHeart + 1;

                    if(numberCard == newAfterFiveHeart)
                    {
                        afterFiveHeart = numberCard;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveHeart = beforeFiveHeart - 1;
                    if(numberCard == newBeforeFiveHeart)
                    {
                        beforeFiveHeart = numberCard;
                    }
                }
            }
        }

        if(deckCard == "Spade")
        {
            if(!fiveSpade) {
                if(numberCard == 5) 
                {
                    fiveSpade = true;
                    beforeFiveSpade = 5;
                    afterFiveSpade = 5;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveSpade = afterFiveSpade + 1;

                    if(numberCard == newAfterFiveSpade)
                    {
                        afterFiveSpade = numberCard;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveSpade = beforeFiveSpade - 1;
                    if(numberCard == newBeforeFiveSpade)
                    {
                        beforeFiveSpade = numberCard;
                    }
                }
            }
        }
    }

    public bool CanHumanUseThisCard(List<KeyValuePair<int, string>> cardSelected)
    {
        int numberCard = cardSelected[0].Key;
        string deckCard = cardSelected[0].Value;
        
        int newBeforeFiveClover;
        int newAfterFiveClover;
        int newBeforeFiveDiamond;
        int newAfterFiveDiamond;
        int newBeforeFiveHeart;
        int newAfterFiveHeart;
        int newBeforeFiveSpade;
        int newAfterFiveSpade;

        if(deckCard == "Clover")
        {
            if(!fiveClover) {
                if(numberCard == 5) 
                {
                    return false;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveClover = afterFiveClover + 1;

                    if(numberCard == newAfterFiveClover)
                    {
                        return false;
                    }
                }
                else if(numberCard < 5)
                {
                    newBeforeFiveClover = beforeFiveClover - 1;

                    if(numberCard == newBeforeFiveClover)
                    {
                        return false;
                    }
                }
            }
        }

        if(deckCard == "Diamond")
        {
            if(!fiveDiamond) {
                if(numberCard == 5) 
                {
                    return false;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveDiamond = afterFiveDiamond + 1;
                    if(numberCard == newAfterFiveDiamond)
                    {
                        return false;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveDiamond = beforeFiveDiamond - 1;
                    if(numberCard == newBeforeFiveDiamond)
                    {
                        return false;
                    }
                }
            }
        }

        if(deckCard == "Heart")
        {
            if(!fiveHeart) {
                if(numberCard == 5) 
                {
                    return false;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveHeart = afterFiveHeart + 1;

                    if(numberCard == newAfterFiveHeart)
                    {
                        return false;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveHeart = beforeFiveHeart - 1;
                    if(numberCard == newBeforeFiveHeart)
                    {
                        return false;
                    }
                }
            }
        }

        if(deckCard == "Spade")
        {
            if(!fiveSpade) {
                if(numberCard == 5) 
                {
                    return false;
                }
            } else {
                if(numberCard > 5)
                {
                    newAfterFiveSpade = afterFiveSpade + 1;

                    if(numberCard == newAfterFiveSpade)
                    {
                        return false;
                    }

                }
                else if(numberCard < 5)
                {
                    newBeforeFiveSpade = beforeFiveSpade - 1;
                    if(numberCard == newBeforeFiveSpade)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public IEnumerator MoveCard(List<KeyValuePair<int, string>> cardSelected)
    {
        if(GameManager.Instance.humanPlay && GameManager.Instance.playerTurn == 1)
        {
            yield return new WaitForSeconds(0.1f);
        } else {
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.audioSource.PlayOneShot(moveCard);
        int numberCard = cardSelected[0].Key;
        string deckCard = cardSelected[0].Value;

        GameObject cardTable = GameObject.Find(numberCard + "_" + deckCard);
        cardTable.GetComponent<SpriteRenderer>().enabled = true;
        DeleteCardFromPlayerDeck(numberCard, deckCard);
        bool stateGame = GameManager.Instance.CheckStateGame();
        if(!stateGame) { PlayerTurn(); }

        yield return null;
    }

    public void DeleteCardFromPlayerDeck(int numberCard, string deckCard)
    {
        GameObject Player = GameObject.Find("playerNumber"+GameManager.Instance.playerTurn);
        var playerDeck = Player.GetComponent<PlayerCard>().playerDeck;
        int index = 0;

        foreach(var playerCard in playerDeck)
        {
            if(playerCard.Key == numberCard && playerCard.Value == deckCard)
            {
                break;
            }
            index++;
        }
        playerDeck.RemoveAt(index);      
    }

    public bool PlayerCheckCards(int playerTurn)
    {
        GameObject PlayerGO = GameObject.Find("playerNumber"+playerTurn);

        List<KeyValuePair<int, string>> listPlayerDeck = PlayerGO.GetComponent<PlayerCard>().playerDeck;

        int newBeforeFiveClover;
        int newAfterFiveClover;
        int newBeforeFiveDiamond;
        int newAfterFiveDiamond;
        int newBeforeFiveHeart;
        int newAfterFiveHeart;
        int newBeforeFiveSpade;
        int newAfterFiveSpade;

        foreach(var playerCard in listPlayerDeck)
        {
            if(playerCard.Value == "Clover")
            {
                if(!fiveClover) {
                    if(playerCard.Key == 5) 
                    {
                        return true;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveClover = afterFiveClover + 1;

                        if(playerCard.Key == newAfterFiveClover)
                        {
                            return true;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveClover = beforeFiveClover - 1;
                        if(playerCard.Key == newBeforeFiveClover)
                        {
                            return true;
                        }
                    }
                }
            }

            if(playerCard.Value == "Diamond")
            {
                if(!fiveDiamond) {
                    if(playerCard.Key == 5) 
                    {
                        return true;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveDiamond = afterFiveDiamond + 1;
                        if(playerCard.Key == newAfterFiveDiamond)
                        {
                            return true;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveDiamond = beforeFiveDiamond - 1;
                        if(playerCard.Key == newBeforeFiveDiamond)
                        {
                            return true;
                        }
                    }
                }
            }

            if(playerCard.Value == "Heart")
            {
                if(!fiveHeart) {
                    if(playerCard.Key == 5) 
                    {
                        return true;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveHeart = afterFiveHeart + 1;

                        if(playerCard.Key == newAfterFiveHeart)
                        {
                            return true;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveHeart = beforeFiveHeart - 1;
                        if(playerCard.Key == newBeforeFiveHeart)
                        {
                            return true;
                        }
                    }
                }
            }

            if(playerCard.Value == "Spade")
            {
                if(!fiveSpade) {
                    if(playerCard.Key == 5) 
                    {
                        return true;
                    }
                } else {
                    if(playerCard.Key > 5)
                    {
                        newAfterFiveSpade = afterFiveSpade + 1;

                        if(playerCard.Key == newAfterFiveSpade)
                        {
                            return true;
                        }

                    }
                    else if(playerCard.Key < 5)
                    {

                        newBeforeFiveSpade = beforeFiveSpade - 1;
                        if(playerCard.Key == newBeforeFiveSpade)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

}
