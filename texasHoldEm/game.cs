using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Game
    {
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
