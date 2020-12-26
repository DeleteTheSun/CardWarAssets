using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cards
{
    public class CardTweening
    {
        public static void PlaceCard(Transform objTransform, Vector3 pos, float time, bool faceUp = false)
        {
            objTransform.DOMove(pos, time);//.OnComplete();
            objTransform.DORotate(objTransform.rotation.eulerAngles + Vector3.up * (faceUp ? 90 : -90), time);
        }
    } 
}
