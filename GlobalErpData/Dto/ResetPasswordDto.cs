using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
