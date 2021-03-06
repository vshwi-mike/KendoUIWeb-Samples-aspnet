﻿using System;
using System.Collections.Generic;
//using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using AutoMapper;
using WebApi.Models;
using WebApi.DTO;

namespace WebApi
{
    public class ProductsController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public ProductsController() {
            Mapper.CreateMap<Category, CategoryDTO>();
            Mapper.CreateMap<Supplier, SupplierDTO>();
            Mapper.CreateMap<Product, ProductDTO>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : ""))
                .ForMember(d => d.SupplierName, o => o.MapFrom(s => s.Supplier != null ? s.Supplier.CompanyName : ""));

            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Products
        public IEnumerable<ProductDTO> GetProducts(
                                        int? category_id = null, 
                                        int? supplier_id = null, 
                                        string product_name = "", 
                                        bool active_only = true) {

            var query = BuildProductListQuery(category_id, supplier_id, product_name, active_only);
            return query.ToList();
        }

        private IQueryable<ProductDTO> BuildProductListQuery(
                                        int? category_id = null,
                                        int? supplier_id = null,
                                        string product_name = "",
                                        bool active_only = true) {
            var query = db.Products
                            .Include("Category")
                            .Include("Supplier")
                            .AsQueryable();

            if ((category_id ?? 0) != 0) {
                query = query.Where(i => i.CategoryID == category_id);
            }

            if ((supplier_id ?? 0) != 0) {
                query = query.Where(i => i.SupplierID == supplier_id);
            }

            if (!string.IsNullOrEmpty(product_name)) {
                query = query.Where(i => i.ProductName.Contains(product_name));
            }

            if (active_only == true) {
                query = query.Where(i => i.Discontinued == false);
            }

            query = query.OrderBy(i => i.ProductID);

            var query2 = query.Select(i => 
                new ProductDTO { 
                    ProductID = i.ProductID ,
                    ProductName = i.ProductName,
                    SupplierID = i.SupplierID,
                    CategoryID = i.CategoryID,
                    QuantityPerUnit = i.QuantityPerUnit,
                    UnitPrice = i.UnitPrice,
                    UnitsInStock = i.UnitsInStock,
                    UnitsOnOrder = i.UnitsOnOrder,
                    ReorderLevel = i.ReorderLevel,
                    Discontinued = i.Discontinued,
                    CategoryName = i.Category != null ? i.Category.CategoryName : "",
                    SupplierName = i.Supplier != null ? i.Supplier.CompanyName : ""
                });
            return query2;
        }

        /// <summary>
        /// Product一覧のページネーション対応版
        /// 
        /// 引数の他にQuery文字列に下記のパラメータが必要。(kendo UI Gridが自動的に付加する。)
        ///     int page        - the page of data item to return (1 means the first page)
        ///     int pageSize    - the number of items to return
        ///     int skip        - how many data items to skip
        ///     int take        - the number of data items to return (the same as pageSize)
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="supplier_id"></param>
        /// <param name="product_name"></param>
        /// <param name="active_only"></param>
        /// <returns></returns>
        [Route("api/ProductsPaged")]
        public PagedList<ProductDTO> GetProductsPaged(
                                        int? category_id = null,
                                        int? supplier_id = null,
                                        string product_name = "",
                                        bool active_only = true) {

            var query = BuildProductListQuery(category_id, supplier_id, product_name, active_only);
            var pagedList = new PagedList<ProductDTO>(Request, query);
            return pagedList;
        }

        // GET: api/Products/5
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products
                            .Include("Category")
                            .Include("Supplier")
                            .SingleOrDefaultAsync(i => i.ProductID == id);
            if (product == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Product, ProductDTO>(product);
            return Ok(dto);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID) {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ProductExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id) {
            Product product = await db.Products.FindAsync(id);
            if (product == null) {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id) {
            return db.Products.Any(e => e.ProductID == id);
        }
    }
}