using ForumPersistence.Entity;
using ForumPersistence.Repositories.Interfaces;

namespace ForumPersistence.Repositories
{
    public class LocalStorage : IStorage
    {
        public async Task SaveToStorageByStreamAsync(string targetFilePath, string fileName, byte[]? content) {
            Directory.CreateDirectory(targetFilePath);
            using (var targetStream = System.IO.File.Create(
                    Path.Combine(targetFilePath, fileName))
            )
            {
                await targetStream.WriteAsync(content);
                targetStream.Close();
            }
        }

        public Task DeleteFileAsync(string path) {
            return Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        }

        public Task<IEnumerable<FileResponse>?> GetFilesAsync(string targetFilePath) {
            return Task.Run(() =>
            {
                if (Directory.Exists(targetFilePath)) {
                    var files = Directory.GetFiles(targetFilePath);
                    if (files.Any())
                    {
                        return files.Select(x => new FileResponse()
                        {
                            FileName = x.Substring(x.LastIndexOf(@"\") + 1),
                            Path = x
                        });
                    }
                }

                return null;
            });
        }

        public Task ClearAsync(string targetFilePath)
        {
            return Task.Run(() =>
            {
                if (Directory.Exists(targetFilePath))
                {
                    Directory.Delete(targetFilePath, true);
                }
            });
        }

        public Task<bool> AnyDirectoryAsync(string path) {
            string validPath = path.Replace(@"\", @"/");
            return Task.Run(() =>
            {
                return Directory.Exists(validPath);
            });
        }

        public Task<bool> AnyFileAsync(string path)
        {
            return Task.Run(() =>
            {
                return File.Exists(path);
            });
        }
    }
}
