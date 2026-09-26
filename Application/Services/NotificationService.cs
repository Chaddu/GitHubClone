using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : INotificationService
{
    public async Task<Result> DeleteAsync(int id, int currentUserId)
    {
        var notif = await unitOfWork.Notifications.GetByIdAsync(id);

        if (notif == null)
            return Result.Failure("Notification was not found", ErrorType.NotFound);

        if (notif.UserId != currentUserId)
            return Result.Failure("You are not allowed to delete this notification", ErrorType.Forbidden);

        unitOfWork.Notifications.Delete(notif);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Notification deleted successfully");
    }

    public async Task<Result<NotificationResponse>> GetByIdAsync(int id, int currentUserId)
    {
        var notif = await unitOfWork.Notifications.GetByIdAsync(id);

        if (notif == null)
            return Result<NotificationResponse>.Failure("Notification was not found", ErrorType.NotFound);

        if (notif.UserId != currentUserId)
            return Result<NotificationResponse>.Failure("You are not allowed to access this notification", ErrorType.Forbidden);
        var response = mapper.Map<NotificationResponse>(notif);

        return Result<NotificationResponse>.Success(response);
    }

    public async Task<Result<List<NotificationResponse>>> GetMyNotificationsAsync(int currentUserId)
    {
        var notif = await unitOfWork.Notifications.GetByUserIdAsync(currentUserId);

        var response = mapper.Map<List<NotificationResponse>>(notif);

        return Result<List<NotificationResponse>>.Success(response);
    }

    public async Task<Result<List<NotificationResponse>>> GetMyUnreadNotificationsAsync(int currentUserId)
    {
        var notif = await unitOfWork.Notifications.GetUnreadByUserIdAsync(currentUserId);

        var resposne = mapper.Map<List<NotificationResponse>>(notif);

        return Result<List<NotificationResponse>>.Success(resposne);
    }

    public async Task<Result> MarkAsReadAsync(int id, int currentUserId)
    {
        var notif = await unitOfWork.Notifications.GetByIdAsync(id);

        if (notif == null)
            return Result.Failure("Notification was not found", ErrorType.NotFound);

        if (notif.UserId != currentUserId)
            return Result.Failure("You are not allowed to modify this notification", ErrorType.Forbidden);

        if (notif.IsRead)
            return Result.Success("Notification is already marled as read");

        notif.IsRead = true;

        unitOfWork.Notifications.Update(notif);
        await unitOfWork.SaveChangesAsync();

        return Result.Success("Notification marked as read.");
    }
}
