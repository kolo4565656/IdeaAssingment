using System.ComponentModel;

namespace ForumPersistence.Exception
{
    public class EnumMappingException : InvalidEnumArgumentException
    {
        public EnumMappingException(string enumName, string? source)
            : base($"Cannot map enum of {enumName} at {source}")
        { }
    }
}
