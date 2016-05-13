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
        public BetChoice ChooseBet(int currentBet)
        {
            BetChoice betChoice = null;

            if (currentBet == 0)
            {
                // Computer can check
                betChoice = new BetChoice(BetChoice.BetActions.Check);
            }
            else if (currentBet <= this.Chips)
            {
                // Computer has enough chips to call
                betChoice = new BetChoice(BetChoice.BetActions.Call, currentBet);
            }
            else
            {
                // Computer cannot do anything but fold
                betChoice = new BetChoice(BetChoice.BetActions.Fold);
            }

            if (betChoice == null)
                return new BetChoice(BetChoice.BetActions.Fold);
            else
            {
                // Normal case
                return betChoice;
            }
        }
        #endregion
    }
}
