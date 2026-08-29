using Application.Common;
using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<User>> GetByIdAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            return Result<User>.Failure("User not found.");

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> GetByUsernameAsync(string username)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);

        if (user is null)
            return Result<User>.Failure("User not found.");

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user is null)
            return Result<User>.Failure("User not found.");

        return Result<User>.Success(user);
    }

    public async Task<Result<bool>> ExistsByUsernameAsync(string username)
    {
        var exists = await _unitOfWork.Users.ExistsByUsernameAsync(username);
        return Result<bool>.Success(exists);
    }

    public async Task<Result<bool>> ExistsByEmailAsync(string email)
    {
        var exists = await _unitOfWork.Users.ExistsByEmailAsync(email);
        return Result<bool>.Success(exists);
    }

    public async Task<Result> AddAsync(User user)
    {
        await _unitOfWork.Users.AddASync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(User user)
    {
        var existingUser = await _unitOfWork.Users.GetByIdAsync(user.Id);

        if (existingUser is null)
            return Result.Failure("User not found.");

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            return Result.Failure("User not found.");

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}

