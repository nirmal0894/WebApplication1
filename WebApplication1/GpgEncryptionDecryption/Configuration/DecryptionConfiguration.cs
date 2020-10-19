using System;
using System.Configuration;

namespace GpgEncryptionDecryption.Contracts
{
    /// <summary>
    /// The decryption configuration contract.
    /// </summary>
    public class DecryptionConfiguration : IDecryptionConfiguration
    {
        private string binaryPath;
        private string gpgHomePath;
        private string passphrase;

        public bool RequiredFromConfiguration;

        public DecryptionConfiguration(bool requiredFromConfiguration)
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
                binaryPath = value;
            }
        }

        public string Passphrase
        {
            get
            {
                return RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgPassphrase"] : passphrase;
            }

            set
            {
                passphrase = value;
            }
        }

        public string GpgHomePath
        {
            get
            {
                return RequiredFromConfiguration ? ConfigurationManager.AppSettings["GpgHomeDirectory"] : gpgHomePath;
            }

            set
            {
                gpgHomePath = value;
            }
        }
    }
}
