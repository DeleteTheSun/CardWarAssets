using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cards
{
    [Serializable]
    public class CardData
    {
        public Sprite FrontImage;
        public CardType Type;
        public CardValue Value;

        public CardData(CardManager.ValueFacePair pair)
        {
            FrontImage = pair.Texture;
            Type = pair.Type;
            Value = pair.Value;
        }
    } 
}
