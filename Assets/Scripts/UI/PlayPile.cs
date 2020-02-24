using System.Collections.Generic;
using UnityEngine;

using Cards;
using UnityEngine.Events;

namespace UI
{
    public class PlayPile : Pile
    {
        public Card GetCurrentTopCard()
        {
            if (_pile.Count > 0)
                return _pile[_pile.Count - 1];
            else
                return null;
        }
    }
}