using SFTPService.Configuration;

namespace SFTPService
{
    public interface ISFTPUploadService
    {
        void UploadFile(ISftpUploadConfiguration sftpUploadConfiguration, string localFilePath, string fileName);
       
    }
}
