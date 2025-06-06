﻿using System.ComponentModel.DataAnnotations;

namespace KanBan.Models
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
