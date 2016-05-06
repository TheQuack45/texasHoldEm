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
            Player humanPlayer = new Player(Game.PossibleGames.TexasHoldEm);
            Computer testComputer = new Computer(Game.PossibleGames.TexasHoldEm);
            pokerGame.AddPlayer(humanPlayer);
            pokerGame.DistributeHands();

            Console.ReadKey();
        }

        public static void GetCurrentBet(int currentBet)
        {
            int betAmount = 0;
            if (currentBet == 0)
            {
                Console.WriteLine("The current bet is {0}. You can 'check', 'raise', or 'fold'.", currentBet);
                while ()
                string actionChoice = Console.ReadLine().ToLower();
                switch (actionChoice)
                {
                    case "check":

                        break;
                    case "raise":

                        break;
                    case "fold":

                        break;
                    default:

                        break;
                }
            }
            else if (currentBet > 0)
            {
                Console.WriteLine("The current bet is {0}. You can 'call', 'raise', or 'fold'.", currentBet);
            }
        }
    }
}
