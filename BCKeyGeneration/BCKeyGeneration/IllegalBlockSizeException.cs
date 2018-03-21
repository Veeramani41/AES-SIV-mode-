using System;
using System.Runtime.Serialization;

namespace BCKeyGeneration
{
    [Serializable]
    internal class IllegalBlockSizeException : Exception
    {
        public IllegalBlockSizeException()
        {
        }

        public IllegalBlockSizeException(string message) : base(message)
        {
        }

        public IllegalBlockSizeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalBlockSizeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}