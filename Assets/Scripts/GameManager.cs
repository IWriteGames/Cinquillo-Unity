using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;
 
    //Preferences Match
    public bool humanPlay {get; private set; }
    public int numberIA {get; private set; }

    //Players Info
    public int playerTurn {get; private set; }
    public int numberPlayers {get; private set; }
    public List<GameObject> playersDeck;

    //Sprite Cards
    public List<Sprite> spriteDeck;

    //Audio
    public AudioSource audioSource;

    //UI
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text textVictory;
    public TMP_Text InfoText;
    [SerializeField] private GameObject humanPlayerCard;
    

    private void Awake()
    {
        Instance = this;

        victoryPanel.SetActive(false);

        //Match Options Selected
        if(PlayerPrefs.GetInt("PlayerPlay") == 1) { humanPlay = true; } else { humanPlay = false; }
        numberIA = PlayerPrefs.GetInt("numberIA");

        //Number Players in Match
        if(humanPlay) 
        { 
            numberPlayers = numberIA + 1; 
            humanPlayerCard.SetActive(true); 
        } 
        else 
        { 
            numberPlayers = numberIA; 
            humanPlayerCard.SetActive(false); 
        }

        for (int i = 1; i <= numberPlayers; i++)
        {
            GameObject playerNumber = new GameObject();
            playerNumber.AddComponent<PlayerCard>();
            playersDeck.Add(playerNumber);
            playerNumber.name = "playerNumber" + i;
        }
    }

    private void Start()
    {
        StartTurn();
    }

    private void StartTurn()
    {
        playerTurn = Random.Range(1, numberPlayers);
        PlayerController.Instance.PlayerTurn();
    }

    public bool HumanPlayerTurn()
    {
        if(humanPlay && playerTurn == 1)
        {
            return true;
        }

        return false;
    }

    public void ChangePlayerTurn()
    {
        playerTurn++;
        if(playerTurn > numberPlayers)
        {
            playerTurn = 1;
        }
    }

    public bool CheckStateGame()
    {
        GameObject Player = null;
        for(int i = 1; i <= numberPlayers; i++)
        {
            Player = GameObject.Find("playerNumber"+i);
            var playerDeck = Player.GetComponent<PlayerCard>().playerDeck;
            if(playerDeck.Count == 0)
            {
                textVictory.text = "Player number " + i + " is the winner!";

                if(humanPlay)
                {
                    if(i == 1)
                    {
                        textVictory.text = "You win!";
                    }
                }
                victoryPanel.SetActive(true);
                return true;
            }
        }

        return false;
    }

}


