using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace ForumPersistence.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AvoidUpdateNull<TEntity>(this EntityEntry entry, TEntity entity, Type[] ignoreProperty = null) {

            Type type = typeof(TEntity);
            PropertyInfo[] properties = type.GetProperties().Where(p =>
                p.PropertyType.Namespace != "System.Collections.Generic" &&
                (p.PropertyType.Name != "ICollection" || p.PropertyType.Name.StartsWith("ICollection`"))
            ).ToArray();

            if (ignoreProperty != null) {
                var relations = ignoreProperty.Select(x => x.Name);
                properties = properties.Where(p => !relations.Contains(p.PropertyType.Name)).ToArray();
            }

            foreach (PropertyInfo property in properties)
            {
                Guid guidHolder;
                var propertyType = property.PropertyType;
                var value = property.GetValue(entity, null);

                if (propertyType.Name == typeof(Guid).Name) {
                    if ((Guid)value == default(Guid)) {
                        entry.Property(property.Name).IsModified = false;
                    }
                }
                else if (value == null)
                {
                    entry.Property(property.Name).IsModified = false;
                }
            }
        }
    }
}
