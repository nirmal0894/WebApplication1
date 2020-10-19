namespace GpgEncryptionDecryption.Contracts
{
    /// <summary>
    /// The decryption configuration contract.
    /// </summary>
    public interface IDecryptionConfiguration : IEncryptionDecryptionConfiguration
    {
        /// <summary>
        /// The passphrase associated with the key
        /// </summary>
        string Passphrase { get; set; }
    }
}
