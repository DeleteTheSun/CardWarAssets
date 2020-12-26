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
        public WarPlayer[] Players;
        public TextMeshProUGUI Text;
        private CardData p1Card, p2Card;
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
            int counter = 0;
            foreach (CardData Card in CardManager.Deck)
            {
                Players[counter].EnqueueCard(Card, TimeToWaitForCard);
                yield return new WaitForSeconds(TimeToWaitForCard);
                counter = counter + 1 < Players.Length ? counter + 1 : 0;
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
            if (!Players[0].TryPlayCard(out p1Card))
            {
                if(Players[1].TryPlayCard(out p2Card))
                {
                    GameEnd("Player 2 wins!");
                }
                else
                {
                    GameEnd("Draw!");

                }
            }
            if (!Players[1].TryPlayCard(out p2Card))
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
                    Text.text = Players[0].Name + " takes the hand!";
                    yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
                    Players[0].EnqueuePool(pool);
                }
                else
                {
                    Text.text = Players[1].Name + " takes the hand!";
                    yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
                    Players[1].EnqueuePool(pool);
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
                CardData p1LastCard, p2LastCard;
                if (!Players[0].TryPlayCard(out p1LastCard, true))
                {
                    if (Players[1].TryPlayCard(out p2LastCard))
                    {
                        GameEnd("Player 2 wins!");
                    }
                    else
                    {
                        //judge by the last card both players played
                        p1Card.FlipCardSideways(WarPlayer.TimeToWaitForCard / 2);
                        p2Card.FlipCardSideways(WarPlayer.TimeToWaitForCard / 2);
                        yield return new WaitForSeconds(WarPlayer.TimeToWaitForCard);
                        if (p1Card.Value > p2Card.Value)
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
                if (!Players[1].TryPlayCard(out p2LastCard, true))
                {
                    GameEnd("Player 1 wins!");
                }
                p1Card = p1LastCard;
                p2Card = p2LastCard;
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