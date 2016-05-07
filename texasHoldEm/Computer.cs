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

        public Computer(Game gameToRegister, string computerName)
        {
            this.RegisterGame(gameToRegister);
            this._playerName = computerName;
        }
        #endregion

        #region Methods definition

        #endregion
    }
}
