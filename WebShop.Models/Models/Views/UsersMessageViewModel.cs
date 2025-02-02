using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models.Models.Views
{
    public class UsersMessageViewModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public ApplicationUser User { get; set; }
        public string Email { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }

    }
}
