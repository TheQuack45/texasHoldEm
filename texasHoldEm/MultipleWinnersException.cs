using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    class MultipleWinnersException : Exception
    {
        private List<Player> _winningPlayers;
        public List<Player> WinningPlayers
        {
            get { return _winningPlayers; }
        }

        // TODO: make serializable when you understand what that is

        public MultipleWinnersException(string message, List<Player> winningPlayers) : base(message)
        {
            this._winningPlayers = winningPlayers;
        }
    }
}
