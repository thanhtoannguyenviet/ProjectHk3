using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoQuanTrong.Common
{
    public class Filter
    {
        public string keyword { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string conditionOrderBy { get; set; }
        public bool orderBy { get; set; }
        public Filter()
        {
            pageNumber = 0;
            pageSize = 3;
            conditionOrderBy = "id";
            keyword = "";
        }

        public Filter(string keyword, int pageNumber, int pageSize, string conditionOrderBy, bool orderBy)
        {
            this.keyword = keyword;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.conditionOrderBy = conditionOrderBy;
            this.orderBy = orderBy;
        }
    }
}