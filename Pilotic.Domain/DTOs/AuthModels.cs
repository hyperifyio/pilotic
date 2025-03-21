using System.ComponentModel.DataAnnotations;

namespace Pilotic.Domain.DTOs;

/// <summary>
/// Request model for requesting a verification code
/// </summary>
public class RequestVerificationCodeRequest
{
    /// <summary>
    /// The email to send the verification code to
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Request model for verifying a code
/// </summary>
public class VerifyCodeRequest
{
    /// <summary>
    /// The email to verify
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The code to verify
    /// </summary>
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response model for authentication
/// </summary>
public class AuthResponse
{
    
    /// <summary>
    /// The token to be used for authentication
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// The expiration date of the token
    /// </summary>
    public DateTime ExpiresAt { get; set; }
} 