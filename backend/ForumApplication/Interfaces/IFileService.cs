using ForumApplication.Dtos;
using ForumPersistence.Entity;
using Microsoft.AspNetCore.Http;

namespace ForumApplication.Interfaces
{
    public interface IFileService
    {
        Task<ResponseResult<FileError>> UploadAttachmentAsync(IList<IFormFile> files, string uniqueFolder);
        Task<ResponseResult<FileError>> UploadMediaFileAsync(IList<IFormFile> files, string uniqueFolder);
        Task DeleteAsync(string path);
        Task<IEnumerable<FileResponse>> GetAttachmentAsync(string uniqueFolder);
        Task<IEnumerable<FileResponse>> GetMediaFileAsync(string uniqueFolder);
        Task ClearAsync(string uniqueFolder, string subFolder = "");
        Task<bool> AnyDirectoryAsync(string path);
        Task<bool> AnyFileAsync(string path);
        Task<ResponseResult<FileError>> UploadByTypeAsync(IList<IFormFile> files, string uniqueFolder, string type);
    }
}
