using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Game
    {
        private Deck _cardDeck;
        public Deck CardDeck
        {
            get { return _cardDeck; }
        }

        public Game()
        {
            this.CardDeck = new Deck();
            CardDeck.Shuffle();
        }
    }
}
