namespace diexpenses.Services.StorageService
{
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;

    public interface IStorageService
    {
        Task<StorageFile> CreateFile(string folderName, string fileName, MemoryStream memoryStream);
        Task<StorageFile> GetFile(string folderName, string fileName);

        Task<StorageFolder> CreateLocalFolder(string folderName);
        Task<StorageFolder> GetLocalFolder(string folderName);
        Task<bool> DeleteLocalFolder(string folderName);
    }
}
