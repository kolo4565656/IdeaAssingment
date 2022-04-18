using ForumPersistence.Constants;
using ForumPersistence.Exception;
using System.Diagnostics;

namespace ForumPersistence.Extensions
{
    public static class MapEnum
    {
        static readonly string Admin = "Admin";
        static readonly string Staff = "Staff";
        public static string MapEnumRole(UserRoles? role) {
            switch (role) {
                case UserRoles.Admin:
                    return Admin;
                case UserRoles.Staff:
                    return Staff;
                default:
                    StackFrame callStack = new StackFrame(1, true);
                    throw new EnumMappingException("Role", callStack.GetFileName());
            }
        }
    }

    
}
