using Domain.Enums;

namespace Application.DTOs.Request;
public class AddRepositoryMemberRequest
{
    public int UserId { get; set; }
    public RepositoryPermission Permission { get; set; }
}
