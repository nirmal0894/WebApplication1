using System;

namespace GpgEncryptionDecryption
{
    public class GpgServiceException : Exception
    {
        public GpgServiceException()
        { }

        public GpgServiceException(string message) : base(message)
        { }

        public GpgServiceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
