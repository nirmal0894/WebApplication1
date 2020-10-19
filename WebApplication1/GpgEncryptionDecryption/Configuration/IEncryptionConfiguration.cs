namespace GpgEncryptionDecryption.Contracts
{
    /// <summary>
    /// The encryption configuration contract.
    /// </summary>
    public interface IEncryptionConfiguration : IEncryptionDecryptionConfiguration
    {
        /// <summary>
        /// The public key associated email id
        /// </summary>
        string Recipient { get; set; }
    }
}
