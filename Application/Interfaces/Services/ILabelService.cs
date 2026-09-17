using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Interfaces.Services;
public interface ILabelService
{
    Task<Result<List<LabelResponse>>> GetByRepositoryIdAsync(int repositoryId);
    Task<Result<LabelResponse>> GetByNameAsync(int repositoryId, string name);
    Task<Result<LabelResponse>> CreateAsync(int repositoryId, CreateLabelRequest request, int currentUserId);
    Task<Result<LabelResponse>> UpdateAsync(int id, UpdateLabelRequest request);
    Task<Result> DeleteAsync(int id);
}
