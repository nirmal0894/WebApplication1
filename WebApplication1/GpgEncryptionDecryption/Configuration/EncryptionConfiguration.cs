using System.Configuration;

namespace GpgEncryptionDecryption.Contracts
{
    /// <summary>
    /// The encryption configuration contract.
    /// </summary>
    public class EncryptionConfiguration : IEncryptionConfiguration
    {
        private string binaryPath;
        private string gpgHomePath;
        private string recipient;

        public bool RequiredFromConfiguration;

        public EncryptionConfiguration(bool requiredFromConfiguration)
        {
            this.RequiredFromConfiguration = requiredFromConfiguration;
        }

        public string BinaryPath
        {
            get
            {
                return RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgBinaryPath"] : binaryPath;
            }

            set
            {
                binaryPath = RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgBinaryPath"] : value;
            }
        }

        public string Recipient
        {
            get
            {
                return RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgRecipient"] : recipient;
            }

            set
            {
                recipient = RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgRecipient"] : value;
            }
        }

        public string GpgHomePath
        {
            get
            {
                ////TBD: Instead of Blank use environment configuration ConfigurationManager.AppSettings["GpgHomeDirectory"]
                return RequiredFromConfiguration ? "" : gpgHomePath;
            }

            set
            {
                gpgHomePath = RequiredFromConfiguration ? "" : value;
            }
        }

    }
}
