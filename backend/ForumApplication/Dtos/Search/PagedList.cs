using Newtonsoft.Json;

namespace ForumApplication.Dtos.Search
{
    public class PagedList<T> where T : class
    {
        [JsonProperty]
        public Paging? Paging
        {
            get;
            set;
        }

        [JsonProperty]
        public IEnumerable<T>? Data
        {
            get;
            set;
        }

        public PagedList()
        {
            Paging = new Paging();
        }
    }
}
