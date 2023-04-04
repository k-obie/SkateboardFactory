﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace SkateFactory4.WebUI.Models;
public class UserViewModel
{
    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string ExistingEmail { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}