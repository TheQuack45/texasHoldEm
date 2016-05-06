using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Computer : Player
    {
        #region Constructors definition
        public Computer()
        {

        }

        public Computer(Game.PossibleGames gameType)
        {
            this.SetHandSize(gameType);
        }
        #endregion

        #region Methods definition

        #endregion
    }
}
