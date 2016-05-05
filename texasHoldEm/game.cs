using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Game
    {
        #region Static members definition
        public static readonly Dictionary<string, int> HoldEmRanks = new Dictionary<string, int>()
        {
            {"2", 1},
            {"3", 2},
            {"4", 3},
            {"5", 4},
            {"6", 5},
            {"7", 6},
            {"8", 7},
            {"9", 8},
            {"10", 9},
            {"Jack", 10},
            {"Queen", 11},
            {"King", 12},
            {"Ace", 13},
        };
        #endregion

        #region Members definition
        private Deck _cardDeck;
        public Deck CardDeck
        {
            get { return _cardDeck; }
        }
        #endregion

        #region Constructors definition
        public Game()
        {
            this._cardDeck = new Deck();
            CardDeck.Shuffle();
        }
        #endregion
    }
}
