using AutoMapper;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Profiles;
public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, OrganizationResponse>();

        CreateMap<CreateOrganizationRequest, Organization>();

        CreateMap<UpdateOrganizationRequest, Organization>();
    }
}
