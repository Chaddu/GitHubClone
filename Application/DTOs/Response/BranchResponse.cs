namespace Application.DTOs.Response;
public class BranchResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int RepositoryId { get; set; }
    public int CreatorId { get; set; }
}
