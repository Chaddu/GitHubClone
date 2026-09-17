using Domain.Enums;

namespace Application.DTOs.Request;

public class UpdateRepositoryMemberPermissionRequest
{
    public RepositoryPermission Permission { get; set; }
}
