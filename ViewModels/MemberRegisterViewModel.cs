﻿using System.ComponentModel.DataAnnotations;

namespace Escape.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
		[Required]
		[MaxLength(20)]
		public string Phone { get; set; }
		[Required]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        [MaxLength(35)]
        public string Surename { get; set; }
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
