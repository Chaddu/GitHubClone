using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class RepositoryProfile : Profile
{
    public RepositoryProfile()
    {
        CreateMap<CreateRepositoryRequest, Repository>();

        CreateMap<UpdateRepositoryRequest, Repository>();

        CreateMap<Repository, RepositoryResponse>();
    }
}
