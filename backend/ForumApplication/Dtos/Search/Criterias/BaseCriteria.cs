using System.Linq.Expressions;

namespace ForumApplication.Dtos.Search
{
    public class BaseCriteria<TEntity>
    {
        public ICollection<Expression<Func<TEntity, bool>>>? Filter { get; set; }
        public Expression<Func<TEntity, int>> SortExps { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 20;
        public string Fields { get; set; }
        public string Sorts { get; set; }
        public ICollection<string> Includes { get; set; }
    }
}
