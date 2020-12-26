using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using DG.Tweening;

namespace War
{
    public class WarPlayer : MonoBehaviour
    {
        public static int TimeToWaitForCard = 1;
        public string Name;
        public Transform PoolTransform;
        public bool IsPlayer;
        public Vector3 NextCardPosition { get; set; }
        public Queue<CardData> Deck { get; set; } = new Queue<CardData>();
        /// <summary>
        /// Trys to deque card into out parameter, will return whether it succeeded or not
        /// </summary>
        /// <param name="cardData">out parameter</param>
        /// <param name="faceDown"></param>
        /// <returns></returns>
        public bool TryPlayCard(out CardData cardData, bool faceDown = false)
        {
            if(Deck.Count > 0)
            {
                cardData = Deck.Dequeue();
                cardData.transform.SetAsLastSibling();
                //Places it in the bottom of the hiearchy so it would be sorted first, appearing as if the first in the card pile.
                cardData.PlaceCard(PoolTransform.position, TimeToWaitForCard, faceDown, !IsPlayer);
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
                EnqueueCard(pool.Dequeue(), TimeToWaitForCard);
            }
        }

        public void EnqueueCard(CardData Card, float time)
        {
            Card.PlaceCard(transform.position, time, true, !IsPlayer);
            //Places it in the top of the hiearchy so it would be sorted last, appearing as if it's below all the other cards.
            Card.transform.SetAsFirstSibling();
            Deck.Enqueue(Card);
        }

    } 
}
