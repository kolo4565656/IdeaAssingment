namespace ForumApplication.Dtos
{
    public class ResponseResult<TError>
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
        public IList<TError> Errors { get; set; }

        public ResponseResult(bool succeeded) {
            Succeeded = succeeded;
            if(Errors == null) Errors = new List<TError>();
        }

        public ResponseResult()
        {
            if (Errors == null) Errors = new List<TError>();
        }

        public void AddError(TError error) {
            if(Succeeded) Succeeded = false;
            Errors.Add(error);
        }
    }

    public class FileError {
        public string Key { get; set; }
        public string Message { get; set; }
        public FileError(string key, string message) { 
            Key = key;
            Message = message;
        }
    }
}
