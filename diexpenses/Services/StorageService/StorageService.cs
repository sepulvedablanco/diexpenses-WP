namespace diexpenses.Services.StorageService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;

    public class StorageService : IStorageService
    {
        public async Task<StorageFile> CreateFile(string folderName, string fileName, MemoryStream memoryStream)
        {
            StorageFolder folder = await CreateLocalFolder(folderName);
            if (folder != null)
            {
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if (file != null)
                {
                    try
                    {
                        var fileBytes = memoryStream.ToArray();
                        using (var s = await file.OpenStreamForWriteAsync())
                        {
                            s.Write(fileBytes, 0, fileBytes.Length);
                        }

                        return file;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public async Task<StorageFile> GetFile(string folderName, string fileName)
        {
            StorageFolder folder = await GetLocalFolder(folderName);
            if (folder != null)
            {
                return await folder.GetFileAsync(fileName);
            }
            return null;
        }

        public async Task<StorageFolder> GetLocalFolder(string folderName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                return await folder.GetFolderAsync(folderName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<StorageFolder> CreateLocalFolder(string folderName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        }

        public async Task<bool> DeleteLocalFolder(string folderName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFolder deleteFolder = await GetLocalFolder(folderName);

            if (deleteFolder != null)
            {
                await deleteFolder.DeleteAsync();
                return true;
            }

            return false;
        }
    }
}
