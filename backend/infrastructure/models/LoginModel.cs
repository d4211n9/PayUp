﻿using System.ComponentModel.DataAnnotations;

namespace api.models;

public class LoginModel
{
    [Required, EmailAddress]
    public string Email { get; set; } //todo create checks for both variables (look in Register model)

    [Required, MinLength(8)] public string Password { get; set; }
}