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
            pokerGame.AddPlayer(humanPlayer);
            pokerGame.DistributeHands();

            Console.ReadKey();
        }
    }
}
