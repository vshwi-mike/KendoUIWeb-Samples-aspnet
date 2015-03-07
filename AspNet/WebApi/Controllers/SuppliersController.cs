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
using AutoMapper;
using WebApi.Models;
using WebApi.DTO;

namespace WebApi
{
    public class SuppliersController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public SuppliersController() {
            Mapper.CreateMap<Supplier, SupplierDTO>();
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Suppliers
        public IEnumerable<SupplierDTO> GetSuppliers() {
            var query = db.Suppliers
                            .OrderBy(i => i.SupplierID);
            var list = Mapper.Map<List<Supplier>, List<SupplierDTO>>(query.ToList());
            return list;
        }

        // GET: api/Suppliers/5
        [ResponseType(typeof(SupplierDTO))]
        public async Task<IHttpActionResult> GetSupplier(int id) {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Supplier, SupplierDTO>(supplier);
            return Ok(dto);
        }


        // PUT: api/Suppliers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupplier(int id, Supplier supplier){
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != supplier.SupplierID) {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!SupplierExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Suppliers
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supplier.SupplierID }, supplier);
        }

        // DELETE: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(int id) {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null) {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id) {
            return db.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}