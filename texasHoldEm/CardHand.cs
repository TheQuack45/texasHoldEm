using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class CardHand
    {
        #region Static members definition
        public enum HandTypes { HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind };
        #endregion

        #region Members definition
        private HandTypes _handType;
        public HandTypes HandType
        {
            get { return _handType; }
            set { _handType = value; }
        }

        private List<Card> _relevantCards;
        public List<Card> RelevantCards
        {
            get { return _relevantCards; }
            set { _relevantCards = value; }
        }
        #endregion

        #region Constructors definition
        public CardHand()
        {
            this._relevantCards = new List<Card>();
        }

        public CardHand(HandTypes handType)
        {
            this._handType = handType;
            this._relevantCards = new List<Card>();
        }

        public CardHand(HandTypes handType, List<Card> relevantCards)
        {
            this._handType = handType;
            this._relevantCards = relevantCards;
        }
        #endregion
    }
}
