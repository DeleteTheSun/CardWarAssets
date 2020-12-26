using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cards
{
    public class CardData : MonoBehaviour
    {
        public Sprite FrontImage;
        public CardType Type;
        public CardValue Value;
        public Image Image;
        private bool faceDown = true, oppositeSide = false;

        public void FillData(CardManager.ValueFacePair pair)
        {
            FrontImage = pair.Texture;
            Type = pair.Type;
            Value = pair.Value;
        }

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
                FlipCardSideways(time/2);
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

        /// <summary>
        /// switches between card faces.
        /// </summary>
        public void SwitchCardFace()
        {
            Image.sprite = faceDown ? FrontImage : CardManager.instance.BackTexture;
            faceDown = !faceDown;
        }
    } 
}
