using ForumApplication.Dtos;
using Microsoft.AspNetCore.Http;


namespace ForumApplication.Extensions
{
    public static class FileHelpers
    {
        // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
        // And wiki(https://en.wikipedia.org/wiki/List_of_file_signatures)
      

        public static async Task<IList<SavingFile>> ProcessStreamedFile(
            ResponseResult<FileError> result, string[] permittedExtensions, int sizeLimit, IList<IFormFile> files, string savingFolder)
        {
            long totalSize = 0;
            IList<SavingFile> fileContents = new List<SavingFile>();
            IList<StreamFile> streams = new List<StreamFile>();

            int currentFile = 1;
            foreach (var file in files) {
                if (file.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0
                    && File.Exists(Path.Combine(savingFolder, file.FileName))) 
                {
                    result.AddError(new FileError("File", "File name is invalid or exist"));
                    return null;
                }
                if (string.IsNullOrEmpty(file.ContentDisposition))
                {
                    result.AddError(new FileError("File", "Invalid form file"));
                    return null;
                }

                Stream stream = file.OpenReadStream();
                totalSize = totalSize + file.Length;
                if (totalSize > sizeLimit * 1048576)
                {
                    result.AddError(new FileError("File", $"The file {file.FileName} exceeds {sizeLimit} MB."));
                    break;
                }

                if (!IsValidFileExtension(file.FileName, permittedExtensions))
                {
                    result.AddError(new FileError("File",
                        "The file type isn't permitted or the file's " +
                        $"signature doesn't match the file's extension at file number {currentFile}"));
                }
                else {
                    streams.Add(new StreamFile { Stream = stream, FileName = file.FileName }); 
                }
            }

            if (result.Succeeded)
            {
                using (var memoryStream = new MemoryStream())
                {
                    foreach (var stream in streams)
                    {
                        await stream.Stream.CopyToAsync(memoryStream);
                        fileContents.Add(new SavingFile { Content = memoryStream.ToArray(), Name = stream.FileName });
                        memoryStream.SetLength(0);
                    }
                }

                if (fileContents.Any()) {
                    return fileContents;
                } 
            }
            else
            {
                streams.Clear();               
            }
            return null;
        }

        private static bool IsValidFileExtension(string fileName, string[] permittedExtensions)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }

            return true;
        }
    }
}
