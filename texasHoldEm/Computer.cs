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

        public Computer(Game gameToRegister)
        {
            this.RegisterGame(gameToRegister);
        }
        #endregion

        #region Methods definition

        #endregion
    }
}
