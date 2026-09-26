namespace Application.DTOs.Response;
public class NotificationResponse
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public int UserId { get; set; }
}
