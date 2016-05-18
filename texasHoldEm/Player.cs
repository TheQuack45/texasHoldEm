using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    abstract class Player
    {
        #region Static members definition
        public static readonly Dictionary<Game.PossibleGames, int> HandSizeDict = new Dictionary<Game.PossibleGames, int>()
        {
            {Game.PossibleGames.TexasHoldEm, 2},
        };
        #endregion

        #region Members definition
        protected Card[] _currentHand;
        public Card[] CurrentHand
        {
            get { return _currentHand; }
        }

        protected int _handSize;
        public int HandSize
        {
            get { return _handSize; }
        }

        protected int _chips;
        public int Chips
        {
            get { return _chips; }
        }

        protected Game _registeredGame;
        public Game RegisteredGame
        {
            get { return _registeredGame; }
        }

        protected string _playerName;
        public string PlayerName
        {
            get { return _playerName; }
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Register the given Game as this Player's game by setting this player's _registeredGame and _handSize members to the appropriate settings.
        /// </summary>
        /// <param name="gameToRegister">Game object to set this Player for</param>
        public void RegisterGame(Game gameToRegister)
        {
            if (this.RegisteredGame == null)
            {
                // This Player does not already have a registered game. Run normally
                if (gameToRegister == null)
                {
                    // Game gameToRegister is null. Throw exception
                    throw new ArgumentNullException("gameToRegister", "The Game to register cannot be null.");
                }

                this._registeredGame = gameToRegister;
                this.SetHandSize(gameToRegister.GameType);
            }
            else
            {
                // This Player is already registered to a game. Throw exception
                throw new RegisteredException("This Player object is already registered to a game. Use UnregisterGame to remove its settings.");
            }
        }

        /// <summary>
        /// Sets this Player's hand size to the appropriate size for the selected game.
        /// </summary>
        /// <param name="gameType">Enum Game.PossibleGames used to choose the appropriate hand size. (eg Hand size in Texas Hold'em is 2)</param>
        protected void SetHandSize(Game.PossibleGames gameType)
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

        /// <summary>
        /// Set this Player's chip amount to `chipCount`
        /// </summary>
        /// <param name="chipCount">Amount to set Player's chip amount to</param>
        public void SetChips(int chipCount)
        {
            this._chips = chipCount;
        }

        /// <summary>
        /// Add `chipCount` to this Player's chip amount
        /// </summary>
        /// <param name="chipCount">Amount to add to this Player's chip amount</param>
        public void AddChips(int chipCount)
        {
            this._chips += chipCount;
        }

        /// <summary>
        /// Subtract `chipCount` from this Player's chip count. Throws ArgumentException if result of subtraction would be negative.
        /// </summary>
        /// <param name="chipCount">Amount to subtract from this Player's chip amount</param>
        public void SubChips(int chipCount)
        {
            if ((this._chips - chipCount) < 0)
            {
                // The chip subtraction would result in a negative amount of chips. Throw exception.
                throw new ArgumentException("The chipCount given would result in a negative chip amount if subtracted.", "chipCount");
            }
            this._chips -= chipCount;
        }

        public List<Card> ReturnCards()
        {
            List<Card> retList = this._currentHand.ToList<Card>();
            this._currentHand = new Card[this._currentHand.Length];

            return retList;
        }
        #endregion

        #region Abstract methods definition
        /// <summary>
        /// Abstract definition to get the next bet from this Player object
        /// </summary>
        /// <param name="currentBet">Int32 of the Game's current required bet amount</param>
        /// <returns>BetChoice object containing info of the choice made</returns>
        public abstract BetChoice MakeBet(int currentBet);
        #endregion
    }

    public class RegisteredException : Exception
    {
        public RegisteredException(string message) : base(message) { }

        protected RegisteredException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}
