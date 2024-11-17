using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ConsultaNFeDto
    {
        public ConsultaNFeDto()
        {
            this.message = string.Empty;
        }

        public ConsultaNFeDto(string message)
        {
            this.message = message;
        }
        
        public string message { get; set; }


    }
}
