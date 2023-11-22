using System.ComponentModel.DataAnnotations;

namespace api.models;

public class InvitableUserSearch
{
    [Required] 
    public string SearchQuery { get; set; }
    [Required]
    public Pagination Pagination { get; set; }
    [Required]
    public int GroupId { get; set; }
}