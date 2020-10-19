using SFTPService.Configuration;

namespace SFTPService
{
    public interface ISFTPDownloadService
    {
        void DownloadFile(ISftpDownloadConfiguration sftpDownloadConfiguration, string localFilePath, string fileName);

        void DeleteFileFromRemoteLocation(ISftpDownloadConfiguration sftpDownloadConfiguration, string fileName);
    }
}
