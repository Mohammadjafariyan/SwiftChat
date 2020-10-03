using System;
using System.Runtime.Serialization;

namespace SignalRMVCChat.Service
{
    [Serializable]
    public class PlanException : Exception
    {
        public PlanException()
        {
        }

        public PlanException(string message) : base(message)
        {
        }

        public PlanException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PlanException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}