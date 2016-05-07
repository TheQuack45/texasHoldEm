﻿using System;
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

        public static readonly Dictionary<PossibleGames, Deck.DeckStyles> GameDeckStyles = new Dictionary<PossibleGames, Deck.DeckStyles>()
        {
            {PossibleGames.TexasHoldEm, Deck.DeckStyles.Standard52},
        };

        public enum PossibleGames { TexasHoldEm };
        #endregion

        #region Members definition
        private Deck _cardDeck;
        public Deck CardDeck
        {
            get { return _cardDeck; }
        }

        private List<Player> _playerList;
        public List<Player> PlayerList
        {
            get { return _playerList; }
        }

        private List<Player> _foldedPlayerList;
        public List<Player> FoldedPlayerList
        {
            get { return _foldedPlayerList; }
        }

        private PossibleGames _gameType;
        public PossibleGames GameType
        {
            get { return _gameType; }
        }

        private int _currentBet;
        public int CurrentBet
        {
            get { return _currentBet; }
        }

        private int _currentPot;
        public int CurrentPot
        {
            get { return _currentPot; }
        }

        public delegate void BetReadyEventHandler(object sender, BetReadyEventArgs args);
        public event BetReadyEventHandler BetReady;
        #endregion

        #region Constructors definition
        public Game(PossibleGames selectedGame)
        {
            this._cardDeck = new Deck(GameDeckStyles[selectedGame]);
            CardDeck.Shuffle();
            this._gameType = selectedGame;
            this._playerList = new List<Player>();
            this._foldedPlayerList = new List<Player>();
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Add the given Player object to the current game's list of players
        /// </summary>
        /// <param name="cPlayer">The Player object to add</param>
        public void AddPlayer(Player cPlayer)
        {
            if (!PlayerList.Contains<Player>(cPlayer))
            {
                PlayerList.Add(cPlayer);
                if (cPlayer.RegisteredGame == null)
                {
                    // Player object is not registered to a Game
                    cPlayer.RegisterGame(this);
                }
            }
        }

        /// <summary>
        /// Distribute cards to each player to fill their hands
        /// </summary>
        public void DistributeHands()
        {
            foreach (Player cPlayer in this.PlayerList)
            {
                for (int i = 1; i <= cPlayer.HandSize; i++)
                {
                    // TODO: error handling
                    cPlayer.AddCardToHand(this.CardDeck.DrawCard());
                }
                cPlayer.AddChips(5);
            }
        }

        /// <summary>
        /// Run through a round of betting, going to each unfolded Player and receive bets
        /// </summary>
        public void PlayBettingRound()
        {
            foreach (Player cPlayer in this.PlayerList)
            {
                if (!this.FoldedPlayerList.Contains(cPlayer))
                {
                    // Player has not folded out of this hand
                    this.GetNextBet(cPlayer);
                }
            }
        }

        private void GetNextBet(Player cPlayer)
        {
            //OnBetReady(new BetReadyEventArgs(cPlayer, this.CurrentBet));
            BetChoice returnedBet = cPlayer.MakeBet(this.CurrentBet);
            if (returnedBet.BetAction == BetChoice.BetActions.Raise)
            {
                // Currently betting Player raised
                this._currentBet = returnedBet.BetAmount;
                this._currentPot += returnedBet.BetAmount;
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Call)
            {
                // Currently betting Player called
                this._currentPot += returnedBet.BetAmount;
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Fold)
            {
                // Currently betting Played folded
                // Add to this round's folded Players list
                this.FoldedPlayerList.Add(cPlayer);
            }
        }

        protected virtual void OnBetReady(BetReadyEventArgs args)
        {
            if (BetReady != null)
            {
                BetReady(this, args);
            }
        }
        #endregion
    }

    class BetReadyEventArgs : EventArgs
    {
        private Player _bettingPlayer;
        public Player BettingPlayer
        {
            get { return _bettingPlayer; }
            set { _bettingPlayer = value; }
        }

        private int _currentBet;
        public int CurrentBet
        {
            get { return _currentBet; }
        }

        public BetReadyEventArgs(Player bettingPlayer, int currentBet)
        {
            this.BettingPlayer = bettingPlayer;
            this._currentBet = currentBet;
        }
    }
}
