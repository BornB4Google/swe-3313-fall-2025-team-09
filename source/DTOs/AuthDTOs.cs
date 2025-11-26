using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class RegisterRequest
{
    [Required]
    [MinLength(1)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;   // un-hashed

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public bool IsAdmin { get; set; }
}
