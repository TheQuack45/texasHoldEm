using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Card
    {
        #region Static members definition
        public static readonly Dictionary<string, string> PosDict = new Dictionary<string, string>()
        {
            {"1", "Ace"},
            {"A", "Ace"},
            {"a", "Ace"},
            {"Ace", "Ace"},
            {"ace", "Ace"},
            {"2", "2"},
            {"3", "3"},
            {"4", "4"},
            {"5", "5"},
            {"6", "6"},
            {"7", "7"},
            {"8", "8"},
            {"9", "9"},
            {"10", "10"},
            {"T", "10"},
            {"t", "10"},
            {"J", "Jack"},
            {"j", "Jack"},
            {"Jack", "Jack"},
            {"jack", "Jack"},
            {"Q", "Queen"},
            {"q", "Queen"},
            {"Queen", "Queen"},
            {"queen", "Queen"},
            {"K", "King"},
            {"k", "King"},
            {"King", "King"},
            {"king", "King"},
        };

        public static readonly Dictionary<string, string> SuitColorDict = new Dictionary<string, string>()
        {
            {"Clubs", "Black"},
            {"clubs", "Black"},
            {"Diamonds", "Red"},
            {"diamonds", "Red"},
            {"Hearts", "Red"},
            {"hearts", "Red"},
            {"Spades", "Black"},
            {"spades", "Black"}, 
        };
        #endregion

        #region Members definition
        private string _suit;
        public string Suit
        {
            get { return _suit; }
        }

        private string _pos;
        public string Pos
        {
            get { return _pos; }
        }
        #endregion

        #region Constructors definition
        public Card(string pos, string suit)
        {
            PosSelect(pos);
            SuitSelect(suit);
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Sets the current Card object's `_pos` (position) member to the correct option.
        /// Throws exception if input is not a valid 52 French deck card position.
        /// </summary>
        /// <param name="pos">Name of card position. Input is checked by the value from putting input into PosDict as a key, so for example, for "Ace", inputs "1", "A", "a", "Ace", and "ace" are all valid.</param>
        private void PosSelect(string pos)
        {
            string posSent = UtilFunc.ToSentenceCase(pos);

            switch (PosDict[posSent])
            {
                case "Ace":
                    this._pos = "Ace";
                    break;
                case "2":
                    this._pos = "2";
                    break;
                case "3":
                    this._pos = "3";
                    break;
                case "4":
                    this._pos = "4";
                    break;
                case "5":
                    this._pos = "5";
                    break;
                case "6":
                    this._pos = "6";
                    break;
                case "7":
                    this._pos = "7";
                    break;
                case "8":
                    this._pos = "8";
                    break;
                case "9":
                    this._pos = "9";
                    break;
                case "10":
                    this._pos = "10";
                    break;
                case "Jack":
                    this._pos = "Jack";
                    break;
                case "Queen":
                    this._pos = "Queen";
                    break;
                case "King":
                    this._pos = "King";
                    break;
                default:
                    throw new ArgumentException(String.Format("The given input {0} was not a valid card position.", pos), "pos");
            }

            return;
        }

        /// <summary>
        /// Sets the current Card object's `_suit` member to the correct option.
        /// Throws exception if input is not a valid 52 French deck card suit.
        /// </summary>
        /// <param name="suit">Name of card suit. Expects full name (eg "Clubs", "Hearts", etc) though it can be in any casing.</param>
        private void SuitSelect(string suit)
        {
            string suitSent = UtilFunc.ToSentenceCase(suit);

            switch (suitSent)
            {
                case "Clubs":
                    this._suit = "Clubs";
                    break;
                case "Diamonds":
                    this._suit = "Diamonds";
                    break;
                case "Hearts":
                    this._suit = "Hearts";
                    break;
                case "Spades":
                    this._suit = "Spades";
                    break;
                default:
                    throw new ArgumentException(String.Format("The given input {0} was not a valid card suit.", suit), "suit");
            }

            return;
        }

        public string GetName()
        {
            return (this.Pos + " of " + this.Suit);
        }
        #endregion
    }
}
