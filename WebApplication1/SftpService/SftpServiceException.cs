using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPService
{
    public class SftpServiceException : Exception
    {
        public SftpServiceException()
        { }

        public SftpServiceException(string message) : base(message)
        { }

        public SftpServiceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
