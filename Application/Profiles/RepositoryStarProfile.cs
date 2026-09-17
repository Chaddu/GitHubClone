using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class RepositoryStarProfile : Profile
{
    public RepositoryStarProfile()
    {
        CreateMap<RepositoryStar, RepositoryStarResponse>();
    }
}
