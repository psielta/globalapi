using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class OldersCanceledDto
    {
        public double PercentageComparedToPreviousMonth { get; set; } = 0;
        public int QuantityOfCancellations { get; set; } = 0;

    }

    public class OldersQuantityDto
    {
        public double PercentageComparedToPrevious { get; set; } = 0;
        public int QuantityOfCategory { get; set; } = 0;

    }

    public class OldersMoneyByMonthDto
    {
        public double PercentageComparedToPrevious { get; set; } = 0;
        public double totalMonth { get; set; } = 0;

    }

    public class MoneyByDayLast7DaysDto
    {
        public DateTime DateTime { get; set; }
        public double totalDay { get; set; } = 0;

    }

    public class OldersMoneyByDayLast7DaysDto
    {
        public List<MoneyByDayLast7DaysDto> Items { get; set; }

    }

    public class Top05ProductsItemDto
    {
        public string Name { get; set; } = "";
        public double Quantity { get; set; } = 0;
        public double Total { get; set; } = 0;

    }

    public class Top05ProductsDto
    {
        public List<Top05ProductsItemDto> Items { get; set; }

    }
}
