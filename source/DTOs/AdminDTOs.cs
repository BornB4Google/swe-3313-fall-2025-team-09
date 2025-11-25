namespace Backend.DTOs;

public class ChangeUserRoleRequest
{
    public bool IsAdmin { get; set; }
}

public class UserSummaryDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}

public class AdminDto
{
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
}
