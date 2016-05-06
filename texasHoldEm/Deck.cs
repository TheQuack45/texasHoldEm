using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Deck
    {
        #region Static members definition
        public enum DeckStyles { Standard52 };
        #endregion

        #region Members and properties definition
        private Stack<Card> _cardStack;
        public Stack<Card> CardStack
        {
            get { return _cardStack; }
        }
        #endregion

        #region Constructors definition
        public Deck(DeckStyles selectedStyle = DeckStyles.Standard52)
        {
            this._cardStack = new Stack<Card>();
            if (selectedStyle == DeckStyles.Standard52)
                PopulateStandardDeck();
        }
        #endregion

        #region Method definition
        /// <summary>
        /// Populates this object's card deck with a standard 52 French set, unshuffled
        /// </summary>
        private void PopulateStandardDeck()
        {
            this._cardStack.Push(new Card("A", "Clubs"));
            this._cardStack.Push(new Card("A", "Diamonds"));
            this._cardStack.Push(new Card("A", "Hearts"));
            this._cardStack.Push(new Card("A", "Spades"));

            this._cardStack.Push(new Card("2", "Clubs"));
            this._cardStack.Push(new Card("2", "Diamonds"));
            this._cardStack.Push(new Card("2", "Hearts"));
            this._cardStack.Push(new Card("2", "Spades"));

            this._cardStack.Push(new Card("3", "Clubs"));
            this._cardStack.Push(new Card("3", "Diamonds"));
            this._cardStack.Push(new Card("3", "Hearts"));
            this._cardStack.Push(new Card("3", "Spades"));

            this._cardStack.Push(new Card("4", "Clubs"));
            this._cardStack.Push(new Card("4", "Diamonds"));
            this._cardStack.Push(new Card("4", "Hearts"));
            this._cardStack.Push(new Card("4", "Spades"));

            this._cardStack.Push(new Card("5", "Clubs"));
            this._cardStack.Push(new Card("5", "Diamonds"));
            this._cardStack.Push(new Card("5", "Hearts"));
            this._cardStack.Push(new Card("5", "Spades"));

            this._cardStack.Push(new Card("6", "Clubs"));
            this._cardStack.Push(new Card("6", "Diamonds"));
            this._cardStack.Push(new Card("6", "Hearts"));
            this._cardStack.Push(new Card("6", "Spades"));

            this._cardStack.Push(new Card("7", "Clubs"));
            this._cardStack.Push(new Card("7", "Diamonds"));
            this._cardStack.Push(new Card("7", "Hearts"));
            this._cardStack.Push(new Card("7", "Spades"));

            this._cardStack.Push(new Card("8", "Clubs"));
            this._cardStack.Push(new Card("8", "Diamonds"));
            this._cardStack.Push(new Card("8", "Hearts"));
            this._cardStack.Push(new Card("8", "Spades"));

            this._cardStack.Push(new Card("9", "Clubs"));
            this._cardStack.Push(new Card("9", "Diamonds"));
            this._cardStack.Push(new Card("9", "Hearts"));
            this._cardStack.Push(new Card("9", "Spades"));

            this._cardStack.Push(new Card("10", "Clubs"));
            this._cardStack.Push(new Card("10", "Diamonds"));
            this._cardStack.Push(new Card("10", "Hearts"));
            this._cardStack.Push(new Card("10", "Spades"));

            this._cardStack.Push(new Card("J", "Clubs"));
            this._cardStack.Push(new Card("J", "Diamonds"));
            this._cardStack.Push(new Card("J", "Hearts"));
            this._cardStack.Push(new Card("J", "Spades"));

            this._cardStack.Push(new Card("Q", "Clubs"));
            this._cardStack.Push(new Card("Q", "Diamonds"));
            this._cardStack.Push(new Card("Q", "Hearts"));
            this._cardStack.Push(new Card("Q", "Spades"));

            this._cardStack.Push(new Card("K", "Clubs"));
            this._cardStack.Push(new Card("K", "Diamonds"));
            this._cardStack.Push(new Card("K", "Hearts"));
            this._cardStack.Push(new Card("K", "Spades"));
        }

        /// <summary>
        /// Shuffles the current Deck object's _cardStack using Richard Durstenfeld's implementation of the Fisher-Yates shuffle
        /// </summary>
        public void Shuffle()
        {
            Random randomGen = new Random();
            Card[] cardArr = CardStack.ToArray<Card>();
            Card[] outputCardArr = new Card[cardArr.Length];

            for (int i = cardArr.Length - 1; i >= 0; i--)
            {
                do
                {
                    int j = randomGen.Next(cardArr.Length);
                    if (!outputCardArr.Contains<Card>(cardArr[j]))
                        outputCardArr[i] = cardArr[j];
                } while (outputCardArr[i] == null);
            }

            _cardStack = BuildStack(outputCardArr);
        }

        /// <summary>
        /// Draws Card from top of CardStack and returns it
        /// </summary>
        /// <returns>Drawn Card object</returns>
        public Card DrawCard()
        {
            return CardStack.Pop();
        }

        /// <summary>
        /// Converts an array of Card objects to a Stack built from the top down
        /// </summary>
        /// <param name="inputArr">Array of Card objects</param>
        /// <returns>Populated Stack of Card objects</returns>
        private Stack<Card> BuildStack(Card[] inputArr)
        {
            Stack<Card> outputStack = new Stack<Card>();

            for (int i = inputArr.Length - 1; i >= 0; i--)
            {
                outputStack.Push(inputArr[i]);
            }

            return outputStack;
        }
        #endregion
    }
}
