using System.IO;

namespace GpgEncryptionDecryption.Service
{
    public interface IGpgService
    {
        /// <summary>
        /// The GPG Encryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="dataToEncrypt">The data that needs to be encrypted.</param>
        /// <param name="recipient">The public key associated email id.</param>
        /// <returns>Encrypted string</returns>
        string Encrypt(string gpgHomePath, string binaryPath, byte[] dataToEncrypt, string recipient);

        /// <summary>
        /// The GPG Encryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="dataToEncrypt">The data that needs to be encrypted.</param>
        /// <param name="recipient">The public key associated email id.</param>
        /// <returns>Encrypted string</returns>
        string Encrypt(string gpgHomePath, string binaryPath, string dataToEncrypt, string recipient);

        /// <summary>
        /// The GPG Decryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="passphrase">The passphrase associated with the key.</param>
        /// <param name="dataToDecrypt">The data that needs to be decrypted.</param>
        /// <returns>Decrypted string</returns>
        string Decrypt(string gpgHomePath, string binaryPath, string passphrase, byte[] dataToDecrypt);

        /// <summary>
        /// The GPG Decryption service.
        /// </summary>
        /// <param name="gpgHomePath">The Home dir for GPG to refer the keys</param>
        /// <param name="binaryPath">Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary.</param>
        /// <param name="passphrase">The passphrase associated with the key.</param>
        /// <param name="dataToDecrypt">The data that needs to be decrypted.</param>
        /// <returns>Decrypted stream</returns>
        MemoryStream DecryptAndReturnStream(string gpgHomePath, string binaryPath, string passphrase, byte[] dataToDecrypt);

        /// <summary>
        /// Reads from file and returns a byte array
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>Byte array of file contents</returns>
        byte[] ReadFromFilePath(string filePath);

        /// <summary>
        /// Writes contents to the file. If file exists, it is over-written.
        /// </summary>
        /// <param name="filePath">The path of the file excluding the file name</param>
        /// <param name="fileName">The name of the file including the extension</param>
        /// <param name="contents">The contents to be written to the file</param>
        void WriteToFilePath(string filePath, string fileName, string contents);
    }
}
