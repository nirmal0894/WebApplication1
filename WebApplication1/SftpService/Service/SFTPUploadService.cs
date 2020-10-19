using Renci.SshNet;
using SFTPService.Configuration;
using System;
using System.IO;
using Tesco.Logging;

namespace SFTPService
{
    public class SFTPUploadService : ISFTPUploadService
    {
        private readonly ILoggingFrameworkAdapter logger;

        public SFTPUploadService(ILoggingFrameworkAdapter logger)
        {
            this.logger = logger;
        }

        public void UploadFile(ISftpUploadConfiguration sftpUploadConfiguration, string localFilePath, string fileName)
        {
            try
            {
                var uploadConfig = sftpUploadConfiguration.SftpUploadConfiguration;
                logger.LogDebug($"Uploading {fileName} file using Sftp to remote location {uploadConfig.RemoteFileLocation}");

                using (var sftpClient = new SftpClient(uploadConfig.Host, uploadConfig.UserName, uploadConfig.Password))
                {
                    sftpClient.Connect();
                    logger.LogDebug($"Connected to host {uploadConfig.Host} file using Sftp");

                    using (var fs = new FileStream(Path.Combine(localFilePath, fileName), FileMode.Open))
                    {
                        sftpClient.BufferSize = 4 * 1024;
                        sftpClient.UploadFile(fs, Path.Combine(uploadConfig.RemoteFileLocation, fileName));
                    }

                    sftpClient.Disconnect();
                    logger.LogDebug($"Upload successful for file {fileName} using Sftp from local location {localFilePath}");

                }
            }
            catch(Exception ex)
            {
                this.logger.LogError($"Error while Sftp Upload", ex);
                throw new SftpServiceException("Error while Sftp Upload", ex);
            }
        }
       
    }

}
