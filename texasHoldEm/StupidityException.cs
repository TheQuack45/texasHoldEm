using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace texasHoldEm
{
    [Serializable]
    class StupidityException : Exception
    {
        public StupidityException(string message) : base(message) { }

        public StupidityException(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt) { }
    }
}
