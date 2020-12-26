using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        private const float TimeToWaitForCard = 0.2f;
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
                yield return new WaitForSeconds(TimeToWaitForCard);
                counter++;
            }
            CanPlay = true;
            Text.text = "Play";
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
            yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
            if(p1Card.Value == p2Card.Value)
            {
                StartCoroutine(War());
            }
            else
            {
                if (p1Card.Value > p2Card.Value)
                {
                    Text.text = Player1.Name + " takes the hand!";
                    yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
                    Player1.EnqueuePool(pool);
                }
                else
                {
                    Text.text = Player2.Name + " takes the hand!";
                    yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
                    Player2.EnqueuePool(pool);
                }
                yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
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
            Text.text = "War!";
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
                        yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
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
                yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
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