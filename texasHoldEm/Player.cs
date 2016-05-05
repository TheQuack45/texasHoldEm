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
        /// Sets this player's hand size to the appropriate size for the selected game
        /// </summary>
        /// <param name="gameType">Enum Game.PossibleGames used to choose the appropriate hand size (eg Hand size in Texas Hold'em is 2)</param>
        public void SetHandSize(Game.PossibleGames gameType)
        {
            int handSize = HandSizeDict[gameType];
            this._handSize = handSize;
            this._currentHand = new Card[handSize];

            return;
        }
        #endregion
    }
}
