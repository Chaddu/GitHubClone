using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class OrganizationMemberProfile : Profile
{
    public OrganizationMemberProfile()
    {
        CreateMap<OrganizationMember, OrganizationMemberResponse>();

        CreateMap<AddOrganizationMemberRequest, OrganizationMember>();

        CreateMap<UpdateOrganizationMemberRoleRequest, OrganizationMember>();
    }
}
