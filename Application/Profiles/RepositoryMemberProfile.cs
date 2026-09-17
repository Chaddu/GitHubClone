using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class RepositoryMemberProfile : Profile
{
    public RepositoryMemberProfile()
    {
        CreateMap<RepositoryMember, RepositoryMemberResponse>();
        CreateMap<AddRepositoryMemberRequest, RepositoryMember>();
        CreateMap<UpdateRepositoryMemberPermissionRequest, RepositoryMember>();
    }
}
