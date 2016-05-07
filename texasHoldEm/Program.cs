using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Program
    {
        static void Main(string[] args)
        {
            Game pokerGame = new Game(Game.PossibleGames.TexasHoldEm);
            Player humanPlayer = new Player(pokerGame);
            //Computer testComputer = new Computer(pokerGame);
            pokerGame.AddPlayer(humanPlayer);
            pokerGame.DistributeHands();
            pokerGame.PlayBettingRound();

            Console.ReadKey();
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
                // TODO: Allow player to specify "raise {number}" or "all-in" and automatically raise by the number specified or go all in
                // There is no current bet. Player cannot call
                Console.WriteLine("The current bet is {0}. You can 'check', 'raise', or 'fold'. You have {1} chips.", currentBet, currentChipCount);
                do
                {
                    string actionChoice = Console.ReadLine().ToLower();
                    switch (actionChoice)
                    {
                        case "check":
                            betChoice = new BetChoice(BetChoice.BetActions.Check);
                            break;
                        case "raise":
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
                                catch (FormatException e)
                                {
                                    Console.WriteLine("Please enter a valid positive integer.");
                                }
                            } while (betAmount == 0);
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
                        default:
                            Console.WriteLine("Bet choice unrecognized. You can 'check', 'raise', or 'fold'.");
                            break;
                    }
                } while (betChoice == null);
            }
            else if (currentBet > 0)
            {
                // Current bet is positive. Player cannot check
                Console.WriteLine("The current bet is {0}. You can 'call', 'raise', or 'fold'. You have {1} chips.", currentBet, currentChipCount);
                do
                {
                    string actionChoice = Console.ReadLine().ToLower();
                    switch (actionChoice)
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
                                catch (FormatException e)
                                {
                                    Console.WriteLine("Please enter a valid positive integer.");
                                }
                            } while (betAmount == 0);
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
                        default:
                            Console.WriteLine("Command unrecognized. You can 'call', 'raise', or 'fold'.");
                            break;
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
