using ForumPersistence.Entity;

namespace ForumPersistence.Repositories.Interfaces
{
    public interface IStorage
    {
        Task SaveToStorageByStreamAsync(string targetFilePath, string fileName, byte[]? content);
        Task DeleteFileAsync(string path);
        Task<IEnumerable<FileResponse>> GetFilesAsync(string targetFilePath);
        Task ClearAsync(string targetFilePath);
        Task<bool> AnyDirectoryAsync(string path);
        Task<bool> AnyFileAsync(string path);

    }
}
