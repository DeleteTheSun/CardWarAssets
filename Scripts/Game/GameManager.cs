using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

namespace War
{
    public class GameManager : Singleton<GameManager>
    {
        private CardManager CardManager;
        public WarPlayer Player1, Player2;
        public TextMeshProUGUI Text;
        private CardData p1Card, p2Card;
        private TweenableCard p1MovingCard, p2MovingCard;
        public Image PlaceHolder1, PlaceHolder2, PlaceHolder3, PlaceHolder4;
        public float TimeToDistributeCard = 0.2f, TimeToMoveCard = 1;
        Queue<CardData> pool = new Queue<CardData>();
        public bool CanPlay { get; set; }
        private void Start()
        {
            CanPlay = false;
            CardManager = CardManager.instance;
            CardManager.Deck.Shuffle();
            StartCoroutine(Distribute());
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0) && CanPlay)
            {
                StartCoroutine(Battle());
            }
        }

        /// <summary>
        /// Distributes the cards to the players
        /// </summary>
        /// <returns></returns>
        private IEnumerator Distribute()
        {
            Text.text = "Wait for cards...";
            int counter = 2;
            WarPlayer player = Player1;
            foreach (CardData Card in CardManager.Deck)
            {
                player = counter % 2 == 0 ? Player1 : Player2;
                player.EnqueueCard(Card, true);
                yield return new WaitForSeconds(TimeToDistributeCard);
                counter++;
            }
            CanPlay = true;
            Text.text = "Left click in order to play";
            CardManager.DeckPlaceHolder.SetActive(false);
        }

        /// <summary>
        /// The main funcion
        /// </summary>
        /// <returns></returns>
        private IEnumerator Battle()
        {
            CanPlay = false;
            if (!Player1.TryPlayCard(out p1Card))
            {
                if(Player2.TryPlayCard(out p2Card))
                {
                    GameEnd("Player 2 wins!");
                }
                else
                {
                    GameEnd("Draw!");

                }
            }
            if (!Player2.TryPlayCard(out p2Card))
            {
                GameEnd("Player 1 wins!");
            }

            pool.Enqueue(p1Card);
            pool.Enqueue(p2Card);
            yield return new WaitForSeconds(TimeToMoveCard);

            if (p1Card.Value == p2Card.Value)
            {
                StartCoroutine(War());
            }
            else
            {
                if (p1Card.Value > p2Card.Value)
                {
                    Text.text = Player1.Name + " takes the hand!";
                    yield return new WaitForSeconds(TimeToMoveCard);
                    Player1.EnqueuePool(pool);
                    Player2.PlayMoveToOpponent();
                }
                else
                {
                    Text.text = Player2.Name + " takes the hand!";
                    yield return new WaitForSeconds(TimeToMoveCard);
                    Player2.EnqueuePool(pool);
                    Player1.PlayMoveToOpponent();
                }

                PlaceHolder1.gameObject.SetActive(false);
                PlaceHolder2.gameObject.SetActive(false);
                PlaceHolder3.gameObject.SetActive(false);
                PlaceHolder4.gameObject.SetActive(false);
                yield return new WaitForSeconds(TimeToMoveCard);
                Text.text = "Play";
            }
            CanPlay = true;
        }

        /// <summary>
        /// Places three cards from each player in the pool, and takes exceptions into account.
        /// </summary>
        /// <returns></returns>
        private IEnumerator War()
        {
            //TimeToMoveCard *= 10;
            //Player1.MyCard.SwitchCardFace();
            //Player2.MyCard.SwitchCardFace();
            Text.text = "War!";
            PlaceHolder1.sprite = p1Card.FrontImage;
            PlaceHolder1.transform.position = Player1.PoolTransform.position;
            PlaceHolder1.gameObject.SetActive(true);
            PlaceHolder1.transform.SetAsLastSibling();

            PlaceHolder2.sprite = p2Card.FrontImage;
            PlaceHolder2.transform.position = Player2.PoolTransform.position;
            PlaceHolder2.gameObject.SetActive(true);
            PlaceHolder2.transform.SetAsLastSibling();

            PlaceHolder3.transform.position = Player1.PoolTransform.position;
            PlaceHolder4.transform.position = Player2.PoolTransform.position;
            for (int i = 0; i < 3; i++)
            {
                if (!Player1.TryPlayCard(out p1Card, true))
                {
                    if (Player2.TryPlayCard(out p2Card))
                    {
                        GameEnd("Player 2 wins!");
                    }
                    else
                    {
                        //judge by the last card both players played
                        Player1.PlayFlipCard();
                        Player2.PlayFlipCard();
                        yield return new WaitForSeconds(TimeToMoveCard);
                        if (p1MovingCard.CardData.Value > p2MovingCard.CardData.Value)
                        {
                            GameEnd("Player 1 wins!");
                        }
                        else if (p1Card.Value > p2Card.Value)
                        {
                            GameEnd("Player 2 wins!");
                        }
                        else
                        {
                            GameEnd("Draw!");
                        }
                    }
                }
                if (!Player2.TryPlayCard(out p2Card, true))
                {
                    GameEnd("Player 1 wins!");
                }

                pool.Enqueue(p1Card);
                pool.Enqueue(p2Card);
                yield return new WaitForSeconds(TimeToMoveCard);

                PlaceHolder3.transform.SetAsLastSibling();
                PlaceHolder4.transform.SetAsLastSibling();
                PlaceHolder3.gameObject.SetActive(true);
                PlaceHolder4.gameObject.SetActive(true);
            }
            CanPlay = true;
            Text.text = "The stakes are high! Play your cards!";
        }

        /// <summary>
        /// Ends the game
        /// </summary>
        /// <param name="result"></param>
        private void GameEnd(string result)
        {
            Text.text = result;
            StopAllCoroutines();
            enabled = false;
        }
    }


}