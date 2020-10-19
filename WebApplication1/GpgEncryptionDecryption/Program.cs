using GpgEncryptionDecryption.Contracts;
using GpgEncryptionDecryption.Service;
using System;
using System.IO;

namespace GpgEncryptionDecryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Encryption Decryption Service");
            var restart = "y";
            do
            {
                Console.WriteLine("1. Encrypt a File");
                Console.WriteLine("2. Decrypt a File");
                Console.WriteLine("Please select your operation by entering 1 or 2");
                var selection = Console.ReadLine();
                ExecuteBasedOnUserInput(selection);

                Console.WriteLine("To continue enter y or Y");
                restart = Console.ReadLine();
            }
            while (restart.Trim().ToLower() == "y");
        }

        private static void ExecuteBasedOnUserInput(string selection)
        {
            try
            {
                IGpgService gpgService = new GpgService();

                switch (selection)
                {
                    case "1":
                        Encrypt(gpgService);
                        break;
                    case "2":
                        Decrypt(gpgService);
                        break;
                    default:
                        Console.WriteLine("Please select one of the option from the menu");
                        break;
                }
            }
            catch (GpgServiceException ex)
            {
                Console.WriteLine($"Exception while executing Gpg service");
                ExceptionDetailsDisplay(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running application");
                ExceptionDetailsDisplay(ex);
            }

        }

        private static void Encrypt(IGpgService gpgService)
        {
            bool overrideConfigurations = OverrideConfigurations();
            IEncryptionConfiguration encryptionConfiguration = new EncryptionConfiguration(!overrideConfigurations);

            if (overrideConfigurations)
            {
                GetGpgHomeDirAndBinaryPath(encryptionConfiguration);

                Console.WriteLine("Enter Recipient");
                encryptionConfiguration.Recipient = Console.ReadLine();
            }

            //// Read File Contents by taking filepath as input from user
            var fileContents = GetFileContents(gpgService, out string filePath);

            ////Encrypt the File Contents
            var encryptedContent = gpgService.Encrypt(encryptionConfiguration.GpgHomePath, encryptionConfiguration.BinaryPath, fileContents, encryptionConfiguration.Recipient);

            ////Write to a file Location
            gpgService.WriteToFilePath(Path.GetDirectoryName(filePath), Path.GetFileName(filePath) + ".asc", encryptedContent);
        }

        private static void Decrypt(IGpgService gpgService)
        {
            var overrideConfigurations = OverrideConfigurations();
            IDecryptionConfiguration decryptionConfiguration = new DecryptionConfiguration(!overrideConfigurations);
            if (overrideConfigurations)
            {
                GetGpgHomeDirAndBinaryPath(decryptionConfiguration);

                Console.WriteLine("Enter Passphrase");
                decryptionConfiguration.Passphrase = Console.ReadLine();
            }

            //// Read File Contents by taking filepath as input from user
            var fileContents = GetFileContents(gpgService, out string filePath);

            ////Decrypt the File Contents
            var decryptedContent = gpgService.Decrypt(decryptionConfiguration.GpgHomePath, decryptionConfiguration.BinaryPath, decryptionConfiguration.Passphrase, fileContents);

            ////Write to a file Location
            gpgService.WriteToFilePath(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath), decryptedContent);
        }

        private static byte[] GetFileContents(IGpgService gpgService, out string filePath)
        {
            Console.WriteLine("Enter location of the file that needs to be encrypted/decrypted");
            var sourceFileLocation = Console.ReadLine();

            filePath = sourceFileLocation;//Path.GetFileName(sourceFileLocation);
            return gpgService.ReadFromFilePath(sourceFileLocation);
        }

        private static void GetGpgHomeDirAndBinaryPath(IEncryptionDecryptionConfiguration encryptionDecryptionConfiguration)
        {
            Console.WriteLine("Enter BinaryPath");
            encryptionDecryptionConfiguration.BinaryPath = Console.ReadLine();

            Console.WriteLine("Enter GpgHomePath");
            encryptionDecryptionConfiguration.GpgHomePath = Console.ReadLine();
        }

        private static bool OverrideConfigurations()
        {
            Console.WriteLine("To override application configurations enter yes(case-insensitive)");
            var overrideFlag = Console.ReadLine();

            var overrrideConfigurations = overrideFlag.Trim().ToLower() == "yes";
            return overrrideConfigurations;
        }

        private static void ExceptionDetailsDisplay(Exception ex)
        {
            Console.WriteLine($"Message {ex.Message}");
            Console.WriteLine($"Stack Trace {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException}");
            }
        }
    }
}
