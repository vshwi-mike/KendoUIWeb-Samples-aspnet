using System;
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
using NorthwindWeb.Models;
using NorthwindWeb.DTO;

namespace NorthwindWeb.Api
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
        public IEnumerable<ProductDTO> GetProducts()
        {
            var query = db.Products
                            .Include("Category")
                            .Include("Supplier")
                            .OrderBy(i => i.ProductID);
            var list = Mapper.Map<List<Product>, List<ProductDTO>>(query.ToList());
            return list;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}