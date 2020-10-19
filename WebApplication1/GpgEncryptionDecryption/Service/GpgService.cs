using Starksoft.Aspen.GnuPG;
using System;
using System.IO;
using System.Text;

namespace GpgEncryptionDecryption.Service
{
    /// To Run this utility:
    /// Update the App Settings in the App.config as per your environment or when you run the application it gives an option 
    /// to provide configurations as input. Configurations to be updated or provided as input include:
    /// 1. GpgBinaryPath : Location to gpg.exe
    /// 2. GpgHomeDirectory : Location of gnupg folder under Users. This is where the information about your imported keys is stored.
    ///                       Incase the application is running with the user who has the access to the default home directory
    ///                       and the keys are imported under the same, please remove the passing of HomeDir parameter while creating
    ///                       the Gpg Object on line 193 and 232.
    /// 3. GpgRecipient : The email Id associated with the GPG key. This would have been given as input while creating the key. 
    ///                   This will be used only while encrypting a file. If only decrypting a file, please ignore this config.
    /// 4. GpgPassphrase : The passphrase(associated with Private Key) associated with the GPG key. 
    ///                    This will be used only while decrypting a file. If only encrypting a file, please ignore this config.
    /// Look for samples in App.config


    /// <summary>
    /// The GPG service class with encyrption and decryption fucntionality
    /// </summary>
    public class GpgService : IGpgService
    {
        #region Constructors

        /// <summary>
        /// The GPG service constructor with logger initialization.
        /// </summary>
        public GpgService()
        { }


        #endregion

        #region Public interface Methods

        /// <summary>
        /// The GPG Encryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="dataToEncrypt">The data that needs to be encrypted.</param>
        /// <param name="recipient">The public key associated email id.</param>
        /// <returns>Encrypted string</returns>
        public string Encrypt(string gpgHomePath, string binaryPath, byte[] dataToEncrypt, string recipient)
        {
            if (dataToEncrypt == null)
            {
                return null;
            }

            return EncryptMessage(binaryPath, dataToEncrypt, recipient, gpgHomePath);
        }

        /// <summary>
        /// The GPG Encryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="dataToEncrypt">The data that needs to be encrypted.</param>
        /// <param name="recipient">The public key associated email id.</param>
        /// <returns>Encrypted string</returns>
        public string Encrypt(string gpgHomePath, string binaryPath, string dataToEncrypt, string recipient)
        {
            if (string.IsNullOrEmpty(dataToEncrypt))
            {
                return null;
            }

            var byteArrayToEncrypt = Encoding.UTF8.GetBytes(dataToEncrypt);

            return EncryptMessage(binaryPath, byteArrayToEncrypt, recipient, gpgHomePath);

        }

        /// <summary>
        /// The GPG Decryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="passphrase">The passphrase associated with the key.</param>
        /// <param name="dataToDecrypt">The data that needs to be decrypted.</param>
        /// <returns>Decrypted string</returns>
        public string Decrypt(string gpgHomePath, string binaryPath, string passphrase, byte[] dataToDecrypt)
        {
            if (dataToDecrypt == null)
            {
                return null;
            }

            try
            {
                string result;
                var encryptedStream = DecryptMessage(binaryPath, passphrase, dataToDecrypt, gpgHomePath);
                using (var reader = new StreamReader(encryptedStream))
                {
                    result = reader.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new GpgServiceException($"Error while decryption: {ex}");
            }
        }

        /// <summary>
        /// The GPG Decryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="passphrase">The passphrase associated with the key.</param>
        /// <param name="dataToDecrypt">The data that needs to be decrypted.</param>
        /// <returns>Decrypted stream</returns>
        public MemoryStream DecryptAndReturnStream(string gpgHomePath, string binaryPath, string passphrase, byte[] dataToDecrypt)
        {
            if (dataToDecrypt == null)
            {
                return null;
            }

            try
            {
                var decryptedStream = DecryptMessage(binaryPath, passphrase, dataToDecrypt, gpgHomePath);
                return decryptedStream;
            }
            catch (Exception ex)
            {
                throw new GpgServiceException($"Error while decryption: {ex}");
            }
        }

        /// <summary>
        /// Reads from file and returns a byte array
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>Byte array of file contents</returns>
        public byte[] ReadFromFilePath(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                throw new GpgServiceException($"Error while reading from file: {ex}");
            }
        }

        /// <summary>
        /// Writes contents to the file. If file exists, it is over-written.
        /// </summary>
        /// <param name="filePath">The path of the file excluding the file name</param>
        /// <param name="fileName">The name of the file including the extension</param>
        /// <param name="contents">The contents to be written to the file</param>
        public void WriteToFilePath(string filePath, string fileName, string contents)
        {
            try
            {
                File.WriteAllText(Path.Combine(filePath,fileName), contents);
            }
            catch (Exception ex)
            {
                throw new GpgServiceException($"Error while writing to the file: {ex}");
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// The GPG encryption core logic
        /// </summary>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="dataToEncrypt">The data that needs to be encrypted.</param>
        /// <param name="recipient">The public key associated email id.</param>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <returns>Encrypted string</returns>
        private string EncryptMessage(string binaryPath, byte[] dataToEncrypt, string recipient, string gpgHomePath)
        {
            try
            {
                var gpg = new Gpg
                {
                    BinaryPath = binaryPath,
                    Recipient = recipient,
                    HomePath = gpgHomePath  
                };
                string result;
                using (var sourceStream = new MemoryStream(dataToEncrypt))
                {
                    using (var encryptedStream = new MemoryStream())
                    {
                        gpg.Encrypt(sourceStream, encryptedStream);
                        encryptedStream.Position = 0;
                        using (var reader = new StreamReader(encryptedStream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new GpgServiceException($"Error while encryption: {ex}");
            }
        }

        /// <summary>
        /// The GPG decryption core logic
        /// </summary>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="passphrase">The passphrase associated with the key.</param>
        /// <param name="dataToDecrypt">The data that needs to be decrypted.</param>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <returns>The memory stream with decrypted data</returns>
        private MemoryStream DecryptMessage(string binaryPath, string passphrase, byte[] dataToDecrypt, string gpgHomePath)
        {
            var encryptedStream = new MemoryStream();
            var gpg = new Gpg
            {
                BinaryPath = binaryPath,
                Passphrase = passphrase,
                HomePath = gpgHomePath
            };

            using (var sourceStream = new MemoryStream(dataToDecrypt))
            {
                gpg.Decrypt(sourceStream, encryptedStream);
                encryptedStream.Position = 0;
                return encryptedStream;
            }
        }

        #endregion
    }
}
