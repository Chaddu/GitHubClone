using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;
public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CreateCommentRequest, Comment>();
        CreateMap<UpdateCommentRequest, Comment>();
        CreateMap<Comment, CommentResponse>();
    }
}
