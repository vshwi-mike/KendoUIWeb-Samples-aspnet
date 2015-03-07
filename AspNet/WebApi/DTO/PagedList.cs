using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace WebApi.DTO {

    public class PageInfo {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }

        public PageInfo() {
            this.page = 1;
            this.pageSize = 10;
            this.skip = 0;
            this.take = this.pageSize;
        }

        public PageInfo(HttpRequestMessage request) : base() {

            var pairs = request.GetQueryNameValuePairs();

            if (pairs.Any(i => i.Key == "page")) {
                this.page = int.Parse(pairs.Single(i => i.Key == "page").Value);
            }
            if (pairs.Any(i => i.Key == "pageSize")) {
                this.pageSize = int.Parse(pairs.Single(i => i.Key == "pageSize").Value);
            }
            if (pairs.Any(i => i.Key == "skip")) {
                this.skip = int.Parse(pairs.Single(i => i.Key == "skip").Value);
            }
            if (pairs.Any(i => i.Key == "take")) {
                this.take = int.Parse(pairs.Single(i => i.Key == "take").Value);
            }
        }
    }

    public class PagedList<T> {

        public PageInfo pageInfo { get; set; }
        public int total { get; set; }
        public ICollection<T> list { get; set; }

        public PagedList (){
            this.pageInfo = new PageInfo();
            this.list = null;
        }

        public PagedList(HttpRequestMessage request, IQueryable<T> query)
            : base() {
            this.pageInfo = new PageInfo(request);

            this.total = query.Count();

            query = query.Skip(this.pageInfo.skip).Take(this.pageInfo.take);
            this.list = query.ToList();
        }

    }
}