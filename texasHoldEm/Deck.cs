using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Deck
    {
        private Stack<Card> _cardList;
        public Stack<Card> CardList
        {
            get { return _cardList; }
        }

        public Deck()
        {
            this._cardList = new Stack<Card>();
        }

        /// <summary>
        /// Populates this object's card deck with a standard 52 set, unshuffled
        /// </summary>
        private void PopulateStandardDeck()
        {

        }
    }
}
