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
        public static readonly List<string> HoldEmRanksList = new List<string>()
        {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "Jack",
            "Queen",
            "King",
            "Ace",
        };

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

        public static readonly Dictionary<PossibleGames, int> CommunityCardsSize = new Dictionary<PossibleGames, int>()
        {
            {PossibleGames.TexasHoldEm, 5},
        };

        public enum PossibleGames { TexasHoldEm };
        #endregion

        #region Members definition
        private Deck _cardDeck;
        public Deck CardDeck
        {
            get { return _cardDeck; }
        }

        private Card[] _communityCards;
        public Card[] CommunityCards
        {
            get { return _communityCards; }
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

        private Player _humanPlayer;
        public Player HumanPlayer
        {
            get { return _humanPlayer; }
        }

        private Computer _computerPlayer;
        public Computer ComputerPlayer
        {
            get { return _computerPlayer; }
        }

        public delegate void BetMadeEventHandler(object sender, BetMadeEventArgs args);
        public event BetMadeEventHandler BetMade;
        #endregion

        #region Constructors definition
        public Game(PossibleGames selectedGame)
        {
            this._cardDeck = new Deck(GameDeckStyles[selectedGame]);
            this._communityCards = new Card[CommunityCardsSize[selectedGame]];
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

                if (cPlayer is Computer)
                {
                    // This is the Computer player object
                    this._computerPlayer = (Computer)cPlayer;
                }
                else
                {
                    // This is the human Player object
                    this._humanPlayer = cPlayer;
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
        /// Run through a round of betting: go to each unfolded Player and receive bets
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

            // Reset _currentBet
            this._currentBet = 0;
        }

        /// <summary>
        /// Gets bet from cPlayer object and applies it to the game's current situation
        /// </summary>
        /// <param name="cPlayer">The Player to get a bet from</param>
        private void GetNextBet(Player cPlayer)
        {
            BetChoice returnedBet = cPlayer.MakeBet(this.CurrentBet);
            if (returnedBet.BetAction == BetChoice.BetActions.Raise)
            {
                // Currently betting Player raised
                OnBetMade(new BetMadeEventArgs(cPlayer.PlayerName, returnedBet.BetAction, returnedBet.BetAmount));
                if (returnedBet.BetAmount >= this.CurrentBet)
                {
                    // Player could match or beat the current bet. Set CurrentBet to the bet made by the Player.
                    this._currentBet = returnedBet.BetAmount;
                    this._currentPot += returnedBet.BetAmount;
                }
                else
                {
                    // Player could not match the current bet. Leave CurrentBet as it was
                    this._currentPot += returnedBet.BetAmount;
                }
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Call)
            {
                // Currently betting Player called
                this._currentPot += returnedBet.BetAmount;
                OnBetMade(new BetMadeEventArgs(cPlayer.PlayerName, returnedBet.BetAction, returnedBet.BetAmount));
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Fold)
            {
                // Currently betting Player folded
                // Add to this round's folded Players list
                this.FoldedPlayerList.Add(cPlayer);
                OnBetMade(new BetMadeEventArgs(cPlayer.PlayerName, returnedBet.BetAction));
            }
            else if (returnedBet.BetAction == BetChoice.BetActions.Check)
            {
                // Currently betting Player checked
                OnBetMade(new BetMadeEventArgs(cPlayer.PlayerName, returnedBet.BetAction, returnedBet.BetAmount));
            }
        }

        /// <summary>
        /// Method for BetMade event triggering
        /// </summary>
        /// <param name="args">BetMadeEventArgs object</param>
        protected virtual void OnBetMade(BetMadeEventArgs args)
        {
            if (BetMade != null)
            {
                BetMade(this, args);
            }
        }

        /// <summary>
        /// Draw three Cards and place them in this Game's community Cards array for the flop
        /// </summary>
        public void DrawFlop()
        {
            this.CardDeck.DrawCard();
            for (int i = 0; i < 3; i++)
            {
                // Loop three times and draw a card each time for the flop
                this.CommunityCards[i] = this.CardDeck.DrawCard();
            }
        }

        /// <summary>
        /// Draw one Card and place it in this Game's community Cards array for the turn
        /// </summary>
        public void DrawTurn()
        {
            this.CardDeck.DrawCard();
            this.CommunityCards[3] = this.CardDeck.DrawCard();
        }

        /// <summary>
        /// Draw one Card and place it in this Game's community Cards array for the river
        /// </summary>
        public void DrawRiver()
        {
            this.CardDeck.DrawCard();
            this.CommunityCards[4] = this.CardDeck.DrawCard();
        }

        /// <summary>
        /// Use List.OrderBy to sort the given list based on rank
        /// </summary>
        /// <param name="cardList">List to sort</param>
        /// <returns>List sorted by rank</returns>
        public List<Card> SortByPos(List<Card> cardList)
        {
            List<Card> orderedList = cardList.OrderByDescending(cCard => HoldEmRanks[cCard.Pos]).ToList<Card>();
            return orderedList;
        }

        /// <summary>
        /// Use List.OrderBy to sort the given list based on suit
        /// </summary>
        /// <param name="cardList">List to sort</param>
        /// <returns>List sorted by suit</returns>
        public List<Card> SortBySuit(List<Card> cardList)
        {
            List<Card> orderedList = cardList.OrderBy(cCard => cCard.Suit).ToList<Card>();
            return orderedList;
        }

        /// <summary>
        /// Add appropriate amount of chips to the winning Player object(s).
        /// </summary>
        public void DecideWinner()
        {
            List<Player> winningPlayers = null;
            Player winner = null ;

            try
            {
                winner = this.FindWinner();
            }
            catch (MultipleWinnersException e)
            {
                winningPlayers = e.WinningPlayers;
            }

            if (winner != null && winningPlayers == null)
            {
                // Single winner
                // Add all the chips from the pot
                winner.AddChips(this.CurrentPot);
            }
            else if (winner == null && winningPlayers != null)
            {
                // Multiple winners
                // Split the pot
                foreach (Player cPlayer in winningPlayers)
                {
                    // To each winning Player, add the result of dividing the pot by amount of Players
                    cPlayer.AddChips((int)(this.CurrentPot / winningPlayers.Count));
                }
            }

            this._currentPot = 0;
        }

        /// <summary>
        /// <para>Checks this Game's list of registered Players and decides which one has the best hand
        /// <para>Throws MultipleWinnersException with a List&lt;Player&gt; containing the winners. Handle this exception to 
        /// </summary>
        /// <returns>Player from this Game with the highest value hand</returns>
        public Player FindWinner()
        {
            List<Player> winningPlayers = new List<Player>();
            Player cWinPlayer = null;
            CardHand cWinPlayerHand = null;

            foreach (Player cPlayer in this.PlayerList)
            {
                CardHand cHand = this.FindHandType(this.GetSevenSet(cPlayer));
                try
                {
                    if ((int)cHand.HandType > (int)cWinPlayerHand.HandType)
                    {
                        // The current hand is better than the previous best hand
                        cWinPlayer = cPlayer;
                        cWinPlayerHand = cHand;
                    }
                    else if ((int)cHand.HandType == (int)cWinPlayerHand.HandType)
                    {
                        // The current hand is equal to the previous best hand
                        // Check if this hand's high card is better than the previous best hand's high card
                        if (HoldEmRanks[cHand.GetHigh().Pos] > HoldEmRanks[cWinPlayerHand.GetHigh().Pos])
                        {
                            // This hand's high card is better than the previous best hand's high card
                            // Set this hand/player as the currne winner
                            cWinPlayer = cPlayer;
                            cWinPlayerHand = cHand;
                        }
                        else if (HoldEmRanks[cHand.GetHigh().Pos] == HoldEmRanks[cWinPlayerHand.GetHigh().Pos])
                        {
                            if (!winningPlayers.Contains<Player>(cWinPlayer))
                            {
                                winningPlayers.Add(cWinPlayer);
                            }
                            winningPlayers.Add(cPlayer);
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    // Should only be triggered on the first Player, so it is safe to change the current winning Player object
                    cWinPlayer = cPlayer;
                    cWinPlayerHand = cHand;
                }
            }

            if (winningPlayers.Count > 0)
            {
                throw new MultipleWinnersException("Multiple players won. Split the pot.", winningPlayers);
            }

            return cWinPlayer;
        }

        /// <summary>
        /// Returns the full set of this Game's community cards and the given Player's hand
        /// </summary>
        /// <param name="inPlayer">Player object to get hand of 2 from</param>
        /// <returns>Full List&lt;Card&gt; of 7</returns>
        public List<Card> GetSevenSet(Player inPlayer)
        {
            return inPlayer.CurrentHand.ToList<Card>().Concat(this.CommunityCards).ToList<Card>();
        }

        /// <summary>
        /// Returns a CardHand object containing info on the hand of the given cardList.
        /// </summary>
        /// <param name="cardList">List&lt;Card&gt;, preferably unsorted</param>
        /// <returns>CardHand object with handType property set to the hand</returns>
        public CardHand FindHandType(List<Card> cardList)
        {
            if (cardList.Count != 7)
            {
                // The list given is not the correct size
                throw new ArgumentException("The List<Card> given was not the correct size (7 cards).", "cardList");
            }
            CardHand hand = new CardHand();
            List<Card> posSortedCards = this.SortByPos(cardList);
            List<Card> suitSortedCards = this.SortBySuit(cardList);

            hand = this.CheckCombos(posSortedCards);
            hand.IsFlush = this.CheckFlush(suitSortedCards);

            if (hand.HandType == CardHand.HandTypes.Straight && hand.IsFlush)
            {
                // Hand may be straight flush
                if (this.CheckFlush(hand.RelevantCards))
                {
                    // Hand is straight flush
                    hand.HandType = CardHand.HandTypes.StraightFlush;
                    hand.RelevantCards = this.GetFlushCards(suitSortedCards);
                }
            }
            
            if (hand.IsFlush && ((int)CardHand.HandTypes.Flush > (int)hand.HandType))
            {
                // If hand is a flush and Flush is a better hand type than what is currently set, change hand type to Flush and change RelevantCards
                hand.HandType = CardHand.HandTypes.Flush;
                hand.RelevantCards = this.GetFlushCards(suitSortedCards);
            }

            return hand;
        }

        /// <summary>
        /// Checks if the given hand (sortedCardList, sorted by suit) is a flush.
        /// </summary>
        /// <param name="sortedCardList">List&lt;Card&gt; sorted by suit</param>
        /// <returns>True if hand is a flush, false if not</returns>
        public bool CheckFlush(List<Card> sortedCardList)
        {
            Card prevCard = null;
            bool isFlush = false;
            int flushSize = 1;

            for (int i = 0; i < sortedCardList.Count; i++)
            {
                try
                {
                    if (sortedCardList[i].Suit == prevCard.Suit)
                    {
                        flushSize++;
                        if (flushSize == 5)
                        {
                            isFlush = true;
                            break;
                        }
                    }
                    else
                    {
                        flushSize = 1;
                    }
                    prevCard = sortedCardList[i];
                }
                catch (NullReferenceException e)
                {
                    prevCard = sortedCardList[i];
                }
            }

            return isFlush;
        }

        /// <summary>
        /// Gets the cards on the given list that are part of a flush.
        /// </summary>
        /// <param name="sortedCardList">List&lt;Card&gt; sorted by suit</param>
        /// <returns>List&lt;Card&gt; with flush cards inside. Returns empty list if there is no flush</returns>
        public List<Card> GetFlushCards(List<Card> sortedCardList)
        {
            List<Card> flushList = new List<Card>();
            Card prevCard = null;
            int flushSize = 1;

            for (int i = 0; i < sortedCardList.Count; i++)
            {
                try
                {
                    if (sortedCardList[i].Suit == prevCard.Suit)
                    {
                        flushSize++;
                        if (flushSize >= 5)
                        {
                            for (int j = i; j >= (i - 4); j--)
                                flushList.Add(sortedCardList[j]);
                            break;
                        }
                    }
                    else
                    {
                        flushSize = 1;
                        flushList.Clear();
                    }
                    prevCard = sortedCardList[i];
                }
                catch (NullReferenceException e)
                {
                    prevCard = sortedCardList[i];
                }
            }

            return flushList;
        }

        /// <summary>
        /// Checks whether hand given (List&lt;Card&gt; sorted by Pos (rank)) is a straight, one pair, two pair, etc.
        /// </summary>
        /// <param name="sortedCardList">List&lt;Card&gt; sorted by Pos</param>
        /// <returns>CardHand object containing hand type and relevant cards</returns>
        public CardHand CheckCombos(List<Card> sortedCardList)
        {
            CardHand cardHand = new CardHand();
            Card prevCard = null;
            int straightSize = 1;

            for (int i = 0; i < sortedCardList.Count; i++)
            {
                try
                {
                    if (HoldEmRanks[prevCard.Pos] == HoldEmRanks[sortedCardList[i].Pos] + 1)
                    {
                        straightSize++;
                        if (straightSize == 5)
                        {
                            // Straight
                            cardHand.HandType = CardHand.HandTypes.Straight;
                            for (int j = i - 1; j >= (i - 4); j--)
                                cardHand.RelevantCards.Add(sortedCardList[j]);
                            cardHand.RelevantCards.Add(sortedCardList[i]);
                        }
                    }
                    else
                    {
                        // Straight has ended
                        // Reset straightSize
                        straightSize = 1;
                    }

                    if (prevCard.Pos == sortedCardList[i].Pos)
                    {
                        // One pair
                        cardHand.RelevantCards.Add(prevCard);
                        cardHand.RelevantCards.Add(sortedCardList[i]);
                        if (i != sortedCardList.Count - 1 && sortedCardList[i + 1].Pos == sortedCardList[i].Pos)
                        {
                            // Three of a kind
                            cardHand.RelevantCards.Add(sortedCardList[i + 1]);
                            if (sortedCardList[i + 2].Pos == sortedCardList[i].Pos)
                            {
                                // Four of a kind
                                cardHand.RelevantCards.Add(sortedCardList[i + 2]);
                                cardHand.HandType = CardHand.HandTypes.FourOfAKind;
                                break;
                            }

                            if (cardHand.HandType == CardHand.HandTypes.OnePair)
                            {
                                // Full house
                                cardHand.HandType = CardHand.HandTypes.FullHouse;
                                break;
                            }
                            cardHand.HandType = CardHand.HandTypes.ThreeOfAKind;
                            prevCard = sortedCardList[i + 2];
                            i += 3;
                            continue;
                        }

                        if (cardHand.HandType == CardHand.HandTypes.ThreeOfAKind)
                        {
                            // Full house
                            cardHand.HandType = CardHand.HandTypes.FullHouse;
                            cardHand.RelevantCards.Add(prevCard);
                            cardHand.RelevantCards.Add(sortedCardList[i]);
                            break;
                        }
                        else if (cardHand.HandType == CardHand.HandTypes.OnePair)
                        {
                            // Two pair
                            cardHand.HandType = CardHand.HandTypes.TwoPair;
                            cardHand.RelevantCards.Add(prevCard);
                            cardHand.RelevantCards.Add(sortedCardList[i]);
                            break;
                        }

                        cardHand.HandType = CardHand.HandTypes.OnePair;
                    }
                    
                    prevCard = sortedCardList[i];
                }
                catch (NullReferenceException e)
                {
                    prevCard = sortedCardList[i];
                    continue;
                }
                catch (FormatException e)
                {
                    throw new StupidityException("This is why you use an enum on card rank and suit, you idiot! If this ever gets thrown, it's time to REFACTOR!");
                }
            }

            if (cardHand.RelevantCards.Count == 0)
            {
                cardHand.HandType = CardHand.HandTypes.HighCard;
                cardHand.RelevantCards.Add(sortedCardList[0]);
            }

            return cardHand;
        }

        public void PrepForNewHand()
        {
            this._currentBet = 0;
            this._foldedPlayerList.Clear();

            foreach (Player cPlayer in this.PlayerList)
            {
                List<Card> cPlayerHand = cPlayer.ReturnCards();
                foreach (Card cCard in cPlayerHand)
                {
                    this._cardDeck.CardStack.Push(cCard);
                }
            }

            this._cardDeck.Shuffle();
        }
        #endregion
    }

    class BetMadeEventArgs : EventArgs
    {
        private string _playerName;
        public string PlayerName
        {
            get { return _playerName; }
        }

        private BetChoice.BetActions _betChoice;
        public BetChoice.BetActions BetChoice
        {
            get { return _betChoice; }
        }

        private int _betAmount;
        public int BetAmount
        {
            get { return _betAmount; }
        }

        public BetMadeEventArgs(string playerName, BetChoice.BetActions betChoice, int betAmount)
        {
            if (betChoice != texasHoldEm.BetChoice.BetActions.Fold)
            {
                // betChoice is the correct type for specifying betAmount
                this._playerName = playerName;
                this._betChoice = betChoice;
                this._betAmount = betAmount;
            }
            else
            {
                // betChoice is not the correct type for specifying betAmount
                // Throw exception
                throw new ArgumentException("betAmount cannot be specified when betChoice is Fold. Use the constructor without 'int betAmount'.", "betAmount");
            }
        }

        public BetMadeEventArgs(string playerName, BetChoice.BetActions betChoice)
        {
            if (betChoice == texasHoldEm.BetChoice.BetActions.Fold)
            {
                // betChoice is the correct type for not specifying betAmount
                this._playerName = playerName;
                this._betChoice = betChoice;
            }
            else
            {
                // betChoice is not the correct type for not specifying betAmount
                // Throw exception
                throw new ArgumentException("You must specify betAmount when betAction is anything other than Fold. Use the constructor with 'int betAmount'.");
            }
        }
    }
}
