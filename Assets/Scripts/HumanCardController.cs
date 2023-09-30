using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanCardController : MonoBehaviour
{
    private int numberCardPlayerSelected = 0;
    private string deckNamePlayerSelected = "";
    [SerializeField] private Image playerCardSelected;
    [SerializeField] private GameObject backgroundCard;
    [SerializeField] private AudioClip humanTouchCard;
    [SerializeField] private Sprite cardBackground;


    private GameObject cardSelectedGO;

    private void Update() 
    {
        HumanSelectCard();
    }

    private void HumanSelectCard()
    {
        if(GameManager.Instance.playerTurn == 1 && GameManager.Instance.humanPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray, 1500f);

                if(hit.collider != null)
                {
                    GameManager.Instance.audioSource.PlayOneShot(humanTouchCard);

                    cardSelectedGO = hit.collider.gameObject;
                    numberCardPlayerSelected = cardSelectedGO.GetComponent<HumanCardTable>().numberCard;
                    deckNamePlayerSelected = cardSelectedGO.GetComponent<HumanCardTable>().deckName;

                    int index = GameManager.Instance.spriteDeck.FindIndex(s => s.name == numberCardPlayerSelected + "_" + deckNamePlayerSelected);
                    playerCardSelected.GetComponent<Image>().sprite = GameManager.Instance.spriteDeck[index];

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

    public void HumanUsedSelectedCard()
    {
        GameManager.Instance.InfoText.text = "";
        playerCardSelected.GetComponent<Image>().sprite = cardBackground;
        List<KeyValuePair<int, string>> cardSelected = new List<KeyValuePair<int, string>>();
        cardSelected.Add(new KeyValuePair<int, string>(numberCardPlayerSelected, deckNamePlayerSelected));

        bool canHumanUseThisCard = PlayerController.Instance.CanHumanUseThisCard(cardSelected);

        if(!canHumanUseThisCard)
        {
            PlayerController.Instance.HumanUpdateValuesTable(cardSelected);

            StartCoroutine(PlayerController.Instance.MoveCard(
                cardSelected
            ));

            Destroy(cardSelectedGO);
        } else {
            GameManager.Instance.InfoText.text = "Prove with another card";
        }
    }

    public void HumanPass()
    {
        if(GameManager.Instance.humanPlay && GameManager.Instance.playerTurn == 1)
        {
            if(!PlayerController.Instance.PlayerCheckCards(GameManager.Instance.playerTurn))
            {
                GameManager.Instance.InfoText.text = "";
                PlayerController.Instance.PlayerTurn();
            } else {
                GameManager.Instance.InfoText.text = "You can't Pass";
            }
        }
    }


}
