namespace GpgEncryptionDecryption.Contracts
{
    /// <summary>
    /// The decryption configuration contract.
    /// </summary>
    public interface IEncryptionDecryptionConfiguration
    {
        /// <summary>
        /// Full path to the gpg, gpg2, gpg.exe or gpg2.exe executable binary
        /// </summary>
        string BinaryPath { get; set; }

        /// <summary>
        /// The Home dir for GPG to refer the keys
        /// </summary>
        string GpgHomePath { get; set; }
    }
}
