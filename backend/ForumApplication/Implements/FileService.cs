using EnsureThat;
using ForumApplication.Constants;
using ForumApplication.Dtos;
using ForumApplication.Extensions;
using ForumApplication.Interfaces;
using ForumPersistence.Entity;
using ForumPersistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO.Compression;

namespace ForumApplication.Implements
{
    public class FileService : IFileService
    {
        private readonly string _baseFolder;
        private readonly IStorage _storage;
        private readonly string[] _permittedExtensions = { ".png", ".jpg", ".jpeg", ".txt", ".mp4", ".mp3", ".gif", ".pdf", ".xlsx", ".docx", ".csv" };
        private readonly int _fileSizeLimit;

        public FileService(
            IStorage storage,
            IConfiguration config
        ) { 
            _storage = storage;
            _fileSizeLimit = config.GetValue<int>("Storage:FileSizeLimit");
            _baseFolder = config.GetValue<string>("Storage:StoredFilesPath");
        }



        public async Task<ResponseResult<FileError>> UploadByTypeAsync(IList<IFormFile> files, string uniqueFolder, string type)
        {

            Ensure.That(uniqueFolder).IsNotNullOrEmpty();
            if (type == FileConstant.AttachmentFolder)
            {
                return await UploadFileAsync(files, Path.Combine(uniqueFolder, FileConstant.AttachmentFolder));
            }
            else if(type == FileConstant.MediaFolder) {
                return await UploadFileAsync(files, Path.Combine(uniqueFolder, FileConstant.MediaFolder));
            }

            return new ResponseResult<FileError>
            {
                Succeeded = false,
                Message = "Directory is wrong"
            };
        }

        public Task<ResponseResult<FileError>>? UploadAttachmentAsync(IList<IFormFile> files, string uniqueFolder)
        {
            Ensure.That(uniqueFolder).IsNotNullOrEmpty();
            return UploadFileAsync(files, Path.Combine(uniqueFolder, FileConstant.AttachmentFolder)).ContinueWith(x => {
                var i = $"{_baseFolder}\\{uniqueFolder}\\";
                ZipFile.CreateFromDirectory($"{_baseFolder}\\{uniqueFolder}\\Attachment", $"{_baseFolder}\\{uniqueFolder}\\attachment.zip");
                return x.Result;
            }); ;
        }

        public Task<ResponseResult<FileError>> UploadMediaFileAsync(IList<IFormFile> files, string uniqueFolder)
        {
            Ensure.That(uniqueFolder).IsNotNullOrEmpty();
            return UploadFileAsync(files, Path.Combine(uniqueFolder, FileConstant.MediaFolder));
        }

        public Task DeleteAsync(string path)
        {
            Ensure.That(path).IsNotNullOrEmpty();
            return _storage.DeleteFileAsync(path);
        }

        public Task ClearAsync(string uniqueFolder, string subFolder = "") {
            switch (subFolder) {
                case FileConstant.AttachmentFolder:
                    uniqueFolder = Path.Combine(_baseFolder, uniqueFolder, FileConstant.AttachmentFolder);
                    break;
                case FileConstant.MediaFolder:
                    uniqueFolder = Path.Combine(_baseFolder, uniqueFolder, FileConstant.MediaFolder);
                    break;
                default:
                    uniqueFolder = Path.Combine(_baseFolder, uniqueFolder);
                    break;
            }

            return _storage.ClearAsync(uniqueFolder);
        }

        public Task<IEnumerable<FileResponse>> GetAttachmentAsync(string uniqueFolder)
        {
            Ensure.That(uniqueFolder).IsNotNullOrEmpty();
            uniqueFolder = Path.Combine(_baseFolder, uniqueFolder);
            return _storage.GetFilesAsync(Path.Combine(uniqueFolder, FileConstant.AttachmentFolder));
        }

        public Task<IEnumerable<FileResponse>> GetMediaFileAsync(string uniqueFolder)
        {
            Ensure.That(uniqueFolder).IsNotNullOrEmpty();
            uniqueFolder = Path.Combine(_baseFolder, uniqueFolder);
            return _storage.GetFilesAsync(Path.Combine(uniqueFolder, FileConstant.MediaFolder));
        }

        public Task<bool> AnyDirectoryAsync(string path)
        {
            return _storage.AnyDirectoryAsync(path);
        }

        public Task<bool> AnyFileAsync(string path)
        {
            return _storage.AnyFileAsync(path);
        }

        async Task<ResponseResult<FileError>> UploadFileAsync(IList<IFormFile> files, string uniqueFolder) {
            try
            {
                uniqueFolder = Path.Combine(_baseFolder, uniqueFolder);
                Ensure.That(files).HasItems();
                ResponseResult<FileError> result = new ResponseResult<FileError>();
                var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                    result, _permittedExtensions, _fileSizeLimit, files, uniqueFolder
                );

                if (!result.Succeeded)
                {
                    return result;
                }

                //save the file to storage
                foreach (var file in streamedFileContent) { 
                    await _storage.SaveToStorageByStreamAsync(uniqueFolder, file.Name, file.Content);             
                }

                return result;
            }
            catch (Exception e) {
                throw new Exception("Something wrong when process upload file", e);
            }
        }
    }
}
