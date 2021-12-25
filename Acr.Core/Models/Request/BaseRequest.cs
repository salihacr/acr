using System;
using System.ComponentModel.DataAnnotations;

namespace Acr.Core.Models.Request
{
    public class BaseRequest
    {
        public BaseRequest()
        {
            LangCode = LanguageCode.Turkish;
        }
        public LanguageCode LangCode { get; set; }
    }
    public class GetRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }
    }
    public class PaginationRequest : BaseRequest
    {
        public PaginationRequest()
        {
            PageNo = 1;
            PageSize = 50;
        }
        [Range(0, 10000)] // max değişebilir
        public int PageNo { get; set; }
        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}