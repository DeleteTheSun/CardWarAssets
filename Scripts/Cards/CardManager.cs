using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Cards
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField]
        private GameObject DeckParent, CardPrefab;
        public List<CardData> Deck;
        public Sprite BackTexture;
        #region Editor
        /// <summary>
        /// struct in order to make it easier to create all the card objects.
        /// </summary>
        [Serializable]
        public struct ValueFacePair
        {
            public CardType Type;
            public CardValue Value;
            public Sprite Texture;

            public ValueFacePair(CardType type, CardValue value, Sprite texture)
            {
                Type = type;
                Value = value;
                Texture = texture;
            }
        }
        public List<ValueFacePair> ValueFacePairs;
        /// <summary>
        /// editor function that automatically creates a list of face value structs containing all the possible cards.
        /// filling the textures needs to be done manually.
        /// </summary>
        [ContextMenu("Reset texture list")]
        public void FillValueFacePair()
        {
            ValueFacePairs.Clear();
            foreach (CardType type in Enum.GetValues(typeof(CardType)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    ValueFacePairs.Add(new ValueFacePair(type, value, null));
                }
            }
        }
        /// <summary>
        /// creates card copies from the list of face value pairs.
        /// </summary>
        [ContextMenu("Make deck from texture list")]
        public void FillDeckFromFaceValuePairs()
        {
            Deck.Clear();
            foreach (ValueFacePair pair in ValueFacePairs)
            {
                Deck.Add(new CardData(pair));
            }
        }
        #endregion

    }
    public enum CardType
    {
        Clubs,
        Diamonds,
        Spades,
        Hearts
    }
    public enum CardValue
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Prince,
        Queen,
        King,
        Ace
    }

}
