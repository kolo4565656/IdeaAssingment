namespace ForumApplication.Dtos.Search
{
    public class BaseModel<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
    }
}
