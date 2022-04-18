namespace ForumApi.Middlewares.Validations
{
    public class ValidationConstants
    {
        //error
        public const string notFoundError = "{0} is not found";
        public const string requireError = "{0} is required";
        public const string existedError = "{0} is existed";
        public const string rangeError = "{0} must be not less than {1} and greater than {2}";
        public const string invalidError = "{0} is invalid";
        public const string duplicateError = "Duplicate {0}";
        public const string minError = "{0} must be not less than {1}";
        public const string maxError = "{0} must be not greater than {1}";
    }
}
