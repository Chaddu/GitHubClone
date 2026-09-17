using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class IssueProfile : Profile
{
    public IssueProfile()
    {
        CreateMap<CreateIssueRequest, Issue>();

        CreateMap<UpdateIssueRequest, Issue>();

        CreateMap<Issue, IssueResponse>();
    }
}
