namespace api.models;

public class Expense
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
}