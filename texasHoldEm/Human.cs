using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class Human : Player
    {
        #region Constructors definition
        public Human() { }

        public Human(string playerName)
        {
            this._playerName = playerName;
        }

        public Human(Game gameToRegister, string playerName)
        {
            this.RegisterGame(gameToRegister);
            this._playerName = playerName;
        }
        #endregion

        #region Methods definition
        /// <summary>
        /// Gets bet from console and makes the appropriate changes to this Player's chip count
        /// </summary>
        /// <param name="currentBet">Game's current bet</param>
        /// <returns>BetChoice object with information about bet made</returns>
        public override BetChoice MakeBet(int currentBet)
        {
            BetChoice betChoice = Program.GetPlayerBet(currentBet, this.Chips);
            if (betChoice.BetAction == BetChoice.BetActions.Call ||
                betChoice.BetAction == BetChoice.BetActions.Raise)
            {
                this._chips -= betChoice.BetAmount;
            }
            return betChoice;
        }
        #endregion
    }
}
