namespace Application.DTOs.Response;
public class ActivityResponse
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int RepositoryId { get; set; }
}
