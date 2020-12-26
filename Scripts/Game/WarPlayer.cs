using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using DG.Tweening;
using TMPro;

namespace War
{
    public class WarPlayer : MonoBehaviour
    {
        public static int TimeToWaitForCard = 1;
        public string Name;
        public Transform PoolTransform;
        public bool IsPlayer;
        public TweenableCard MyCard;
        public GameObject DeckObj;
        public TextMeshProUGUI Text;
        //public Vector3 NextCardPosition { get; set; }
        public Queue<CardData> Deck { get; set; } = new Queue<CardData>();

        private void Start()
        {
            DeckObj.transform.position = transform.position;
        }
        /// <summary>
        /// Trys to deque card into out parameter, will return whether it succeeded or not
        /// </summary>
        /// <param name="cardData">out parameter</param>
        /// <param name="faceDown"></param>
        /// <returns></returns>
        public bool TryPlayCard(out CardData cardData, bool War = false)
        {
            if(Deck.Count > 0)
            {
                cardData = Deck.Dequeue();
                MyCard.CardData = cardData;
                MyCard.transform.SetAsLastSibling();
                if (!War)
                {
                    PlayMoveToArena();
                }
                else
                {
                    MyCard.SwitchCardFace(true);
                    MyCard.transform.position = transform.position;
                    PlayWar();
                }
                Text.text = Deck.Count.ToString();
                return true; 
            }
            else
            {
                cardData = null;
                return false;
            }
        }

        /// <summary>
        /// Adds the pool into the queue.
        /// </summary>
        /// <param name="pool"></param>
        public void EnqueuePool(Queue<CardData> pool)
        {
            while (pool.Count > 0)
            {
                EnqueueCard(pool.Dequeue());
            }
        }

        public void EnqueueCard(CardData Card, bool quick = false)
        {
            MyCard.transform.SetAsFirstSibling();
            if (quick)
            {
                PlayDistribute();
            }
            else
            {
                PlayMoveToDeck();
            }
            Deck.Enqueue(Card);
            Text.text = Deck.Count.ToString();
            DeckObj.SetActive(Deck.Count>0);
        }
        public void PlayFlipCard()
        {
            if (MyCard.FlipCard.playedOnce)
            {
                MyCard.FlipCard.Restart();
            }
            else
            {
                MyCard.FlipCard.Play();
            }
        }

        public void PlayMoveToDeck()
        {
            if (MyCard.MoveToDeck.playedOnce)
            {
                MyCard.MoveToDeck.Restart();
            }
            else
            {
                MyCard.MoveToDeck.Play();
            }
        }
        public void PlayMoveToArena()
        {
            if (MyCard.MoveToArena.playedOnce)
            {
                MyCard.MoveToArena.Restart();
            }
            else
            {
                MyCard.MoveToArena.Play();
            }
        }
        public void PlayMoveToOpponent()
        {
            if (MyCard.MoveToOpponent.playedOnce)
            {
                MyCard.MoveToOpponent.Restart();
            }
            else
            {
                MyCard.MoveToOpponent.Play();
            }
            MyCard.transform.SetAsFirstSibling();
        }
        public void PlayDistribute()
        {
            if (MyCard.Distribute.playedOnce)
            {
                MyCard.Distribute.Restart();
            }
            else
            {
                MyCard.Distribute.Play();
            }
        }
        public void PlayWar()
        {
            if (MyCard.War.playedOnce)
            {
                MyCard.War.Restart();
            }
            else
            {
                MyCard.War.Play();
            }
        }
    } 
}
