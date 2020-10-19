using Renci.SshNet;
using SFTPService.Configuration;
using System;
using System.IO;
using Tesco.Logging;

namespace SFTPService
{
    public class SFTPDownloadService : ISFTPDownloadService
    {
        private readonly ILoggingFrameworkAdapter logger;

        public SFTPDownloadService(ILoggingFrameworkAdapter logger)
        {
            this.logger = logger;
        }

        public void DownloadFile(ISftpDownloadConfiguration sftpDownloadConfiguration, string localFilePath, string fileName)
        {
            try
            {
                var downloadConfigs = sftpDownloadConfiguration.SftpDownloadConfiguration;
                logger.LogDebug($"Downloading {fileName} file using Sftp from remote location {downloadConfigs.RemoteFileLocation}");

                using (SftpClient sftp = new SftpClient(downloadConfigs.Host, downloadConfigs.UserName, downloadConfigs.Password))
                {
                    sftp.Connect();
                    logger.LogDebug($"Connected to host {downloadConfigs.Host} file using Sftp");

                    using (Stream fileStream = File.OpenWrite(Path.Combine(localFilePath, fileName)))
                    {
                        sftp.DownloadFile(Path.Combine(downloadConfigs.RemoteFileLocation, fileName), fileStream);
                    }

                    sftp.Disconnect();
                    logger.LogDebug($"Download successful for file {fileName} using Sftp to local location {localFilePath}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while Sftp Download", ex);
                throw new SftpServiceException("Error while Sftp Download", ex);
            }
        }

        public void DeleteFileFromRemoteLocation(ISftpDownloadConfiguration sftpDownloadConfiguration, string fileName)
        {
            try
            {
                var downloadConfigs = sftpDownloadConfiguration.SftpDownloadConfiguration;
                logger.LogDebug($"Deleting {fileName} file using Sftp from remote location {downloadConfigs.RemoteFileLocation}");

                using (SftpClient sftp = new SftpClient(downloadConfigs.Host, downloadConfigs.UserName, downloadConfigs.Password))
                {
                    sftp.Connect();
                    logger.LogDebug($"Connected to host {downloadConfigs.Host} file using Sftp");
                   
                    sftp.Delete(Path.Combine(downloadConfigs.RemoteFileLocation, fileName));

                    sftp.Disconnect();
                    logger.LogDebug($"Delete successful for file {fileName}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while Sftp Delete", ex);
                throw new SftpServiceException("Error while Sftp Delete", ex);
            }
        }
    }

}
