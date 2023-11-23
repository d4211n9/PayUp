namespace test;

public class Models
{
    public class Group
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}