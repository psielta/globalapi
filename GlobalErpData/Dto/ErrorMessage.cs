using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Detail { get; set; }

        public ErrorMessage(int statusCode, string message, string? detail = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detail = detail;
        }
    }
}
