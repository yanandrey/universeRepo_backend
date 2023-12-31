﻿namespace UniverseRepo.Application.DTOs.Response;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}