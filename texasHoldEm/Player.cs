using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Player
    {
        #region Static members definition
        public static readonly Dictionary<Game.PossibleGames, int> HandSizeDict = new Dictionary<Game.PossibleGames, int>()
        {
            {Game.PossibleGames.TexasHoldEm, 2},
        };
        #endregion

        #region Members definition
        private Card[] _currentHand;
        public Card[] CurrentHand
        {
            get { return _currentHand; }
        }

        private int _handSize;
        public int HandSize
        {
            get { return _handSize; }
        }
        #endregion

        #region Constructors definition
        public Player()
        {
            
        }

        public Player(Game.PossibleGames gameType)
        {
            this.SetHandSize(gameType);
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Sets this Player's hand size to the appropriate size for the selected game.
        /// </summary>
        /// <param name="gameType">Enum Game.PossibleGames used to choose the appropriate hand size. (eg Hand size in Texas Hold'em is 2)</param>
        public void SetHandSize(Game.PossibleGames gameType)
        {
            int handSize = HandSizeDict[gameType];
            this._handSize = handSize;
            this._currentHand = new Card[handSize];

            return;
        }

        /// <summary>
        /// Add the given Card to this Player's hand. Throws HandFullException if there is no space in the hand.
        /// </summary>
        /// <param name="cCard">The Card to add to the Player's hand</param>
        public void AddCardToHand(Card cCard)
        {
            if (!this.CurrentHand.Contains<Card>(cCard))
            {
                for (int i = 0; i < this.HandSize; i++)
                {
                    if (this.CurrentHand[i] == null)
                    {
                        this.CurrentHand[i] = cCard;
                        return;
                    }
                }

                throw new HandFullException("The Card could not be added because the current Player's hand is full");
            }
        }
        #endregion
    }
}
