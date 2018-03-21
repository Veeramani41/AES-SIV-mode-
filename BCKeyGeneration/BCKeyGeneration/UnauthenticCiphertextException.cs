using System;
using System.Runtime.Serialization;

namespace BCKeyGeneration
{
    [Serializable]
    internal class UnauthenticCiphertextException : Exception
    {
        public UnauthenticCiphertextException()
        {
        }

        public UnauthenticCiphertextException(string message) : base(message)
        {
        }

        public UnauthenticCiphertextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnauthenticCiphertextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}