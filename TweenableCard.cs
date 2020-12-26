using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using War;

namespace Cards
{
    public class TweenableCard : MonoBehaviour
    {
        public CardData CardData;
        public Image Image;
        public GameObject Deck;
        public WarPlayer Player, Opponent;
        public Sequence MoveToDeck, MoveToArena, MoveToOpponent, Distribute, FlipCard;
        public float TimeToPlace = 1, TimeToGoToDeck = 0.2f;
        private bool faceDown = true;

        private void Start()
        {
            MoveToDeck = DOTween.Sequence().SetAutoKill(false);
            MoveToDeck.Pause();
            MoveToDeck.Append(transform.DOMove(Player.transform.position, TimeToPlace));//.OnComplete(ReturnToPosition);

            MoveToOpponent = DOTween.Sequence().SetAutoKill(false);
            MoveToOpponent.Pause();
            MoveToOpponent.Append(transform.DOMove(Opponent.transform.position, TimeToPlace));//.OnComplete(ReturnToPosition);
            MoveToOpponent.Join(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 90, 90), TimeToPlace / 2).OnComplete(SwitchCardFace));
            MoveToOpponent.AppendInterval(TimeToPlace / 2);
            MoveToOpponent.Append(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -90, 90), TimeToPlace / 2));

            MoveToArena = DOTween.Sequence().SetAutoKill(false);
            MoveToArena.Pause();
            MoveToArena.Append(transform.DOMove(Player.PoolTransform.position, TimeToPlace));
            MoveToArena.Join(transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, TimeToPlace / 2).OnComplete(SwitchCardFace));
            MoveToArena.AppendInterval(TimeToPlace / 2);
            MoveToArena.Append(transform.DORotate(transform.rotation.eulerAngles + Vector3.up * -90, TimeToPlace / 2));

            Distribute = DOTween.Sequence().SetAutoKill(false);
            Distribute.Pause();
            Distribute.Append(transform.DOMove(Player.transform.position, TimeToGoToDeck));//.SetLoops(CardManager.instance.Deck.Count/2, LoopType.Restart));

            FlipCard = DOTween.Sequence().SetAutoKill(false);
            FlipCard.Pause();
            FlipCard.Append(transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, TimeToPlace / 2).OnComplete(SwitchCardFace));
            FlipCard.AppendInterval(TimeToPlace / 2);
            FlipCard.Append(transform.DORotate(transform.rotation.eulerAngles + Vector3.up * -90, TimeToPlace / 2));
        }

        #region old
        /*
        /// <summary>
        /// moves the card to the given position.
        /// </summary>
        /// <param name="pos">the position the card will end up in</param>
        /// <param name="time">the time it takes to reach the position</param>
        /// <param name="turnFaceDown">whether the card should be face down or not</param>
        public void PlaceCard(Vector3 pos, float time, bool turnFaceDown, bool turnOppositeSide)
        {
            transform.DOMove(pos, time);
            //if(oppositeSide != turnOppositeSide)
            //{
            //    transform.DORotate(transform.rotation.eulerAngles + Vector3.forward * 180 * (oppositeSide ? -1 : 1), time);
            //    oppositeSide = turnOppositeSide;
            //} 

            if (turnFaceDown != faceDown)
            {
                FlipCardSideways(time / 2);
            }
        }

        /// <summary>
        /// flips the card and changes the texture in order to make it appear like the card had been flipped.
        /// calls itself a second time after the first flip.
        /// </summary>
        /// <param name="time">the time it takes for the card to finish one half turn, the function will take twice the time to finish</param>
        /// <param name="secondTurn">wheter it's the second iteration or not</param>
        public void FlipCardSideways(float time, bool secondTurn = false)
        {
            //transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, time).SetLoops(2, LoopType.Yoyo).OnComplete(SwitchCardFace);
            if (!secondTurn)
            {
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * 90, time).
                    OnComplete(() => FlipCardSideways(time, true));
            }
            else
            {
                SwitchCardFace();
                transform.DORotate(transform.rotation.eulerAngles + Vector3.up * -90, time);
            }
        } 
        */
        #endregion

        /// <summary>
        /// switches between card faces.
        /// </summary>
        public void SwitchCardFace()
        {
            faceDown = !faceDown;
            Image.sprite = CardData.FrontImage;
        }

        public void ReturnToPosition()
        {
            transform.position = Player.transform.position;
        }

    }
}
