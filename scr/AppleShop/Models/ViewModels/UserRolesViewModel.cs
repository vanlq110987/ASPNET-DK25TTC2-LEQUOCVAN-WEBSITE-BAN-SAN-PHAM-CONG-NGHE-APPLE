using System;
using System.Collections.Generic;

namespace AppleShop.Models.ViewModels
{
    /// <summary>Một dòng người dùng trong trang quản lý tài khoản của Admin.</summary>
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsLockedOut { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
