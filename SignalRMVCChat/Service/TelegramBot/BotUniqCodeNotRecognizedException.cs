using System;
using System.Runtime.Serialization;

namespace SignalRMVCChat.Service.TelegramBot
{
    [Serializable]
    internal class BotUniqCodeNotRecognizedException : Exception
    {
        public BotUniqCodeNotRecognizedException()
        {
        }

        public BotUniqCodeNotRecognizedException(string message) : base(message)
        {
        }

        public BotUniqCodeNotRecognizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BotUniqCodeNotRecognizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}