using Domain.Enums;

namespace Application.DTOs.Response;
public class RepositoryMemberResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RepositoryId { get; set; }
    public RepositoryPermission Permission { get; set; }
}
