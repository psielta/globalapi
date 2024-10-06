using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string href { get; set; }
        public string imageSrc { get; set; }
        public string imageAlt { get; set; }
        public string price { get; set; }
        public decimal priceNumber { get; set; }
    }

    public class ProductImage
    {
        public int id { get; set; }
        public string name { get; set; }
        public string src { get; set; }
        public string alt { get; set; }
    }

    public class ProductColor
    {
        public string name { get; set; }
        public string bgColor { get; set; }
        public string selectedColor { get; set; }
    }

    public class ProductDetailv2
    {
        public string name { get; set; }
        public List<string> items { get; set; }
    }

    public class ProductDetails : Product
    {
        public int rating { get; set; }
        public List<ProductImage> images { get; set; }
        public List<ProductColor> colors { get; set; }
        public string description { get; set; }
        public List<ProductDetailv2> details { get; set; }
    }

    // PagedResponse class
    //public class PagedResponse<T>
    //{
    //    public List<T> Items { get; set; }
    //    public int PageNumber { get; set; }
    //    public int PageSize { get; set; }
    //    public int TotalCount { get; set; }
    //
    //    public PagedResponse(List<T> items, int pageNumber, int pageSize, int totalCount)
    //    {
    //        Items = items;
    //        PageNumber = pageNumber;
    //        PageSize = pageSize;
    //        TotalCount = totalCount;
    //    }
    //}
}
