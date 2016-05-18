using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Program
    {
        static void Main(string[] args)
        {
            Game pokerGame = new Game(Game.PossibleGames.TexasHoldEm);
            pokerGame.BetMade += new Game.BetMadeEventHandler(InformBetAction);
            pokerGame.HandFinished += new Game.HandFinishedEventHandler(InformHandFinished);
            Human humanPlayer = new Human(pokerGame, "Human");
            Computer testComputer = new Computer(pokerGame, "Computer");
            pokerGame.AddPlayer(humanPlayer);
            pokerGame.AddPlayer(testComputer);
            pokerGame.DistributeChips(5);

            PlayGame(pokerGame);
        }

        /// <summary>
        /// DEBUG method: Alternative Main() that allows manual checking of input hands
        /// </summary>
        /// <param name="args"></param>
        public static void FalseMain(string[] args)
        {
            Console.WriteLine("Which cards to check?");
            List<Card> cardList = new List<Card>();
            for (int i = 0; i < 7; i++)
            {
                string tmp = Console.ReadLine();
                cardList.Add(new Card(Regex.Split(tmp, " of ")[0], Regex.Split(tmp, " of ")[1]));
            }
            Game testGame = new Game(Game.PossibleGames.TexasHoldEm);
            CardHand outHand = testGame.FindHandType(testGame.SortByPos(cardList));

            Console.WriteLine("outHand type: {0}", outHand.HandType);
            foreach (Card cCard in outHand.RelevantCards)
            {
                Console.WriteLine(cCard.GetName());
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Perform the actual gameplay. Disgusting hack job
        /// </summary>
        /// <param name="pokerGame">The Game object to work with</param>
        public static void PlayGame(Game pokerGame)
        {
            string playerResponse = "";

            do
            {
                Console.Clear();

                pokerGame.CardDeck.Shuffle();
                pokerGame.DistributeHands();
                Console.WriteLine("Your current hand is:");
                Card debugCard = pokerGame.HumanPlayer.CurrentHand[0];
                foreach (Card cCard in pokerGame.HumanPlayer.CurrentHand)
                {
                    // Go through each Card in human Player's hand
                    Console.WriteLine(cCard.GetName());
                }
                Console.WriteLine("");

                Console.WriteLine("The current pot is {0}.", pokerGame.CurrentPot);
                pokerGame.PlayBettingRound();
                Console.WriteLine("");

                // The flop
                pokerGame.DrawFlop();
                Console.WriteLine("The flop has been drawn. The community cards are:");
                DispCommunityCards(pokerGame);
                Console.WriteLine("");

                Console.WriteLine("The current pot is {0}.", pokerGame.CurrentPot);
                pokerGame.PlayBettingRound();
                Console.WriteLine("");

                // The turn
                pokerGame.DrawTurn();
                Console.WriteLine("The turn has been drawn. The community cards are:");
                DispCommunityCards(pokerGame);
                Console.WriteLine("");

                Console.WriteLine("The current pot is {0}.", pokerGame.CurrentPot);
                pokerGame.PlayBettingRound();
                Console.WriteLine("");

                // The river
                pokerGame.DrawRiver();
                Console.WriteLine("The river has been drawn. The community cards are:");
                DispCommunityCards(pokerGame);
                Console.WriteLine("");

                Console.WriteLine("The current pot is {0}.", pokerGame.CurrentPot);
                pokerGame.PlayBettingRound();
                Console.WriteLine("");

                // Decide winner
                int currentPot = pokerGame.CurrentPot;
                try
                {
                    Player winningPlayer = pokerGame.DecideWinner();
                    Console.WriteLine("{0} won that hand, winning {1} chips.", winningPlayer.PlayerName, currentPot);
                }
                catch (MultipleWinnersException e)
                {
                    Console.WriteLine("Multiple players won that hand. Winners: ");
                    foreach (Player cPlayer in e.WinningPlayers)
                    {
                        Console.Write(cPlayer.PlayerName + ", ");
                    }
                    Console.WriteLine(".\r\nThe pot was split between the players, with each winning {0} chips.", (int)(currentPot / e.WinningPlayers.Count));
                }

                pokerGame.PrepForNewHand();

                Console.WriteLine("Do you want to play another round? (y/n)");
                do
                {
                    playerResponse = Console.ReadLine();
                } while (playerResponse == "");
            } while (playerResponse == "y");

            Console.WriteLine("Press Enter to exit the game.");
            Console.ReadKey();
        }

        public static void InformHandFinished(object sender, HandFinishedEventArgs args)
        {
            string playerResponse = "";

            Console.WriteLine("Every other player folded out of the hand, so {0} won that hand, winning {1} chips." + args.PlayerName, args.CurrentPot);
            do
            {
                Console.WriteLine("Do you want to play another round? (y/n)");

                playerResponse = Console.ReadLine();
                if (playerResponse == "y" || playerResponse == "n")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("I don't understand that command.");
                }
            } while (playerResponse == "");

            if (playerResponse == "y")
            {
                PlayGame((Game)sender);
            }
            else
            {
                Console.WriteLine("Press Enter to exit the game.");
                Console.ReadKey();
            }
        }

        public static void InformBetAction(object sender, BetMadeEventArgs args)
        {
            Game cGame = (Game)sender;
            if (args.PlayerName != cGame.HumanPlayer.PlayerName) {
                // Betting player is not the human player
                if (args.BetChoice == BetChoice.BetActions.Fold)
                {
                    // Player acting folded
                    Console.WriteLine("{0} folded out of the hand.", args.PlayerName);
                }
                else if (args.BetChoice == BetChoice.BetActions.Check)
                {
                    // Player acting checked
                    Console.WriteLine("{0} checked.", args.PlayerName);
                }
                else if (args.BetChoice == BetChoice.BetActions.Call)
                {
                    // Player acting called
                    Console.WriteLine("{0} called for {1} chips.", args.PlayerName, args.BetAmount);
                }
                else if (args.BetChoice == BetChoice.BetActions.Raise)
                {
                    // Player acting raised
                    Console.WriteLine("{0} raised for {1} chips, making the current bet {2} chips.", args.PlayerName, (args.BetAmount - cGame.CurrentBet), args.BetAmount);
                }
            }
        }

        public static void DispCommunityCards(Game currentGame)
        {
            foreach (Card cCard in currentGame.CommunityCards)
            {
                // List each card in the community card set
                try
                {
                    Console.WriteLine(cCard.GetName());
                }
                catch (NullReferenceException e)
                {
                    // There are no more cards in the community set
                    break;
                }
            }
        }

        /// <summary>
        /// Get amount to bet from the human player through console.
        /// </summary>
        /// <param name="currentBet">The current bet in the game. 0 if this player can check.</param>
        /// <param name="bettingPlayer">The player object betting. Used to check if the player is trying to bet more than is available.</param>
        /// <returns></returns>
        public static BetChoice GetPlayerBet(int currentBet, int currentChipCount)
        {
            BetChoice betChoice = null;
            int betAmount = 0;
            
            if (currentBet == 0)
            {
                // There is no current bet. Player cannot call
                Console.WriteLine("The current bet is {0}. You can 'check', 'raise [num]', go 'all-in', or 'fold'. You have {1} chips.", currentBet, currentChipCount);
                do
                {
                    string[] actionChoiceArr = Regex.Split(Console.ReadLine().ToLower(), " ");
                    try {
                        switch (actionChoiceArr[0])
                        {
                            case "check":
                                betChoice = new BetChoice(BetChoice.BetActions.Check);
                                break;
                            case "raise":
                                try
                                {
                                    // Player specified amount to raise by "raise {number}"
                                    bool isPreSpecRaiseNumInvalid = false;
                                    do
                                    {
                                        if (!isPreSpecRaiseNumInvalid)
                                        {
                                            // Check Player's specified raise number normally
                                            string betAmountString = actionChoiceArr[1];
                                            try
                                            {
                                                betAmount = Int32.Parse(betAmountString);
                                                if (betAmount > currentChipCount)
                                                {
                                                    // Player tried to bet more than they have available
                                                    Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                    isPreSpecRaiseNumInvalid = true;
                                                    betAmount = 0;
                                                }
                                                else if (betAmount < 0)
                                                {
                                                    // Player tried to bet a negative amount of chips
                                                    Console.WriteLine("You cannot bet a negative amount of chips.");
                                                    isPreSpecRaiseNumInvalid = true;
                                                    betAmount = 0;
                                                }
                                            }
                                            catch (FormatException parseE)
                                            {
                                                Console.WriteLine("Please enter a valid positive integer.");
                                                isPreSpecRaiseNumInvalid = true;
                                            }
                                        }
                                        else
                                        {
                                            // Player's pre-specified raise num is invalid. Get new one from player
                                            Console.WriteLine("How much would you like to raise? (positive integer or 'all-in')");
                                            do
                                            {
                                                try
                                                {
                                                    string betAmountString = Console.ReadLine().ToLower();
                                                    if (betAmountString == "all-in")
                                                    {
                                                        betAmount = currentChipCount;
                                                    }
                                                    else
                                                    {
                                                        betAmount = Int32.Parse(betAmountString);
                                                        if (betAmount > currentChipCount)
                                                        {
                                                            // Player tried to bet more than they have available
                                                            Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                            betAmount = 0;
                                                        }
                                                        else if (betAmount < 0)
                                                        {
                                                            // Player tried to bet a negative amount of chips
                                                            Console.WriteLine("You cannot bet a negative amount of chips.");
                                                            betAmount = 0;
                                                        }
                                                    }
                                                }
                                                catch (FormatException parseE)
                                                {
                                                    Console.WriteLine("Please enter a valid positive integer.");
                                                }
                                            } while (betAmount == 0);
                                        }
                                    } while (betAmount == 0);
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                    // Player did not specify amount to raise by "raise {number}". Get number to raise from player
                                    Console.WriteLine("How much would you like to raise? (positive integer or 'all-in')");
                                    do
                                    {
                                        try
                                        {
                                            string betAmountString = Console.ReadLine().ToLower();
                                            if (betAmountString == "all-in")
                                            {
                                                betAmount = currentChipCount;
                                            }
                                            else
                                            {
                                                betAmount = Int32.Parse(betAmountString);
                                                if (betAmount > currentChipCount)
                                                {
                                                    // Player tried to bet more than they have available
                                                    Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                    betAmount = 0;
                                                }
                                                else if (betAmount < 0)
                                                {
                                                    // Player tried to bet a negative amount of chips
                                                    Console.WriteLine("You cannot bet a negative amount of chips.");
                                                    betAmount = 0;
                                                }
                                            }
                                        }
                                        catch (FormatException parseE)
                                        {
                                            Console.WriteLine("Please enter a valid positive integer.");
                                        }
                                    } while (betAmount == 0);
                                }

                                if (betAmount > 0)
                                {
                                    betChoice = new BetChoice(BetChoice.BetActions.Raise, betAmount);
                                }
                                else
                                {
                                    betChoice = new BetChoice(BetChoice.BetActions.Check);
                                }
                                break;
                            case "fold":
                                betChoice = new BetChoice(BetChoice.BetActions.Fold);
                                break;
                            case "all-in":
                                betChoice = new BetChoice(BetChoice.BetActions.Raise, currentChipCount);
                                break;
                            default:
                                Console.WriteLine("Bet choice unrecognized. You can 'check', 'raise', or 'fold'.");
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Command unrecognized. You can 'check', 'raise [num]', or 'fold'.");
                    }
                } while (betChoice == null);
            }
            else if (currentBet > 0)
            {
                // Current bet is positive. Player cannot check
                Console.WriteLine("The current bet is {0}. You can 'call', 'raise [num]', go 'all-in', or 'fold'. You have {1} chips.", currentBet, currentChipCount);
                do
                {
                    string[] actionChoiceArr = Regex.Split(Console.ReadLine().ToLower(), " ");
                    try
                    {
                        switch (actionChoiceArr[0])
                        {
                            case "call":
                                if (currentBet > currentChipCount)
                                {
                                    // The Player cannot afford to match the current bet. Go all-in
                                    betChoice = new BetChoice(BetChoice.BetActions.Call, currentChipCount);
                                }
                                else
                                {
                                    // The Player can afford to match the current bet
                                    betChoice = new BetChoice(BetChoice.BetActions.Call, currentBet);
                                }
                                break;
                            case "raise":
                                try
                                {
                                    // Player specified amount to raise by "raise {number}"
                                    bool isPreSpecRaiseNumInvalid = false;
                                    do
                                    {
                                        if (!isPreSpecRaiseNumInvalid)
                                        {
                                            // Check Player's specified raise number normally
                                            string betAmountString = actionChoiceArr[1];
                                            try
                                            {
                                                betAmount = Int32.Parse(betAmountString);
                                                if (betAmount > currentChipCount)
                                                {
                                                    // Player tried to bet more than they have available
                                                    Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                    isPreSpecRaiseNumInvalid = true;
                                                    betAmount = 0;
                                                }
                                                else if (betAmount < 0)
                                                {
                                                    // Player tried to bet a negative amount of chips
                                                    Console.WriteLine("You cannot bet a negative amount of chips.");
                                                    isPreSpecRaiseNumInvalid = true;
                                                    betAmount = 0;
                                                }
                                            }
                                            catch (FormatException parseE)
                                            {
                                                Console.WriteLine("Please enter a valid positive integer.");
                                                isPreSpecRaiseNumInvalid = true;
                                            }
                                        }
                                        else
                                        {
                                            // Player's pre-specified raise num is invalid. Get new one from player
                                            Console.WriteLine("How much would you like to raise? (positive integer or 'all-in')");
                                            do
                                            {
                                                try
                                                {
                                                    string betAmountString = Console.ReadLine().ToLower();
                                                    if (betAmountString == "all-in")
                                                    {
                                                        betAmount = currentChipCount;
                                                    }
                                                    else
                                                    {
                                                        betAmount = Int32.Parse(betAmountString);
                                                        if (betAmount > currentChipCount)
                                                        {
                                                            // Player tried to bet more than they have available
                                                            Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                            betAmount = 0;
                                                        }
                                                        else if (betAmount < 0)
                                                        {
                                                            // Player tried to bet a negative amount of chips
                                                            Console.WriteLine("You cannot bet a negative amount of chips.");
                                                            betAmount = 0;
                                                        }
                                                    }
                                                }
                                                catch (FormatException parseE)
                                                {
                                                    Console.WriteLine("Please enter a valid positive integer.");
                                                }
                                            } while (betAmount == 0);
                                        }
                                    } while (betAmount == 0);
                                }
                                catch (IndexOutOfRangeException e)
                                {
                                    // Player did not specify amount to raise by "raise {number}". Get number to raise from player
                                    Console.WriteLine("How much would you like to raise? (positive integer or 'all-in')");
                                    do
                                    {
                                        try
                                        {
                                            string betAmountString = Console.ReadLine().ToLower();
                                            if (betAmountString == "all-in")
                                            {
                                                betAmount = currentChipCount;
                                            }
                                            else
                                            {
                                                betAmount = Int32.Parse(betAmountString);
                                                if (betAmount > currentChipCount)
                                                {
                                                    // Player tried to bet more than they have available
                                                    Console.WriteLine("You cannot bet more chips than you have. You have {0} chips available.", currentChipCount);
                                                    betAmount = 0;
                                                }
                                                else if (betAmount < 0)
                                                {
                                                    // Player tried to bet a negative amount of chips
                                                    Console.WriteLine("You cannot bet a negative amount of chips.");
                                                    betAmount = 0;
                                                }
                                            }
                                        }
                                        catch (FormatException parseE)
                                        {
                                            Console.WriteLine("Please enter a valid positive integer.");
                                        }
                                    } while (betAmount == 0);
                                }

                                if (betAmount > 0)
                                {
                                    betChoice = new BetChoice(BetChoice.BetActions.Raise, betAmount);
                                }
                                else
                                {
                                    betChoice = new BetChoice(BetChoice.BetActions.Check);
                                }
                                break;
                            case "fold":
                                betChoice = new BetChoice(BetChoice.BetActions.Fold);
                                break;
                            case "all-in":
                                betChoice = new BetChoice(BetChoice.BetActions.Raise, currentChipCount);
                                break;
                            default:
                                Console.WriteLine("Command unrecognized. You can 'call', 'raise', or 'fold'.");
                                break;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Command unrecognized. You can 'call', 'raise [num]', or 'fold'.");
                    }
                } while (betChoice == null);
            }
            else
            {
                // Current bet is negative. Throw exception
                throw new ArgumentException("The currentBet integer cannot be negative.", "currentBet");
            }

            return betChoice;
        }
    }
}
