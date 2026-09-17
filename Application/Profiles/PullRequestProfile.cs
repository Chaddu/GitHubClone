using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class PullRequestProfile : Profile
{
    public PullRequestProfile()
    {
        CreateMap<CreatePullRequestRequest, PullRequest>();
        CreateMap<UpdatePullRequestRequest, PullRequest>();
        CreateMap<PullRequest, PullRequestResponse>();
    }   
}
