﻿﻿using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber {  get; set; } = string.Empty;

        public string? Role { get; set; } = string.Empty;
    }
}
