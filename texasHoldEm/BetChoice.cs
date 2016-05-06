using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class BetChoice
    {
        #region Static members definition
        public enum BetActions { Check, Call, Raise, Fold }
        #endregion

        #region Members definition
        private BetActions _betAction;
        public BetActions BetAction
        {
            get { return _betAction; }
        }

        private int _betAmount;
        public int BetAmount
        {
            get { return _betAmount; }
        }
        #endregion

        #region Constructors definition
        public BetChoice(BetActions betAction)
        {
            if (betAction == BetActions.Check || betAction == BetActions.Fold || betAction == BetActions.Call)
            this._betAction = betAction;
        }

        public BetChoice(BetActions betAction, int betAmount)
        {
            // TODO: continue this
        }
        #endregion
    }
}
