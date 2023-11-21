namespace api.models;

public class Expense
{
    public int Id { get; set; }
    public int Group_Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Created_Date { get; set; }
}