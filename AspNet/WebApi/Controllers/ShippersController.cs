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
using NorthwindWeb.Models;
using NorthwindWeb.DTO;

namespace NorthwindWeb.Api
{
    public class ShippersController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public ShippersController() {
            Mapper.CreateMap<Shipper, ShipperDTO>();
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Shippers
        public IEnumerable<ShipperDTO> GetShippers()
        {
            var query = db.Shippers
                            .OrderBy(i => i.ShipperID);
            var list = Mapper.Map<List<Shipper>, List<ShipperDTO>>(query.ToList());
            return list;
        }

        // GET: api/Shippers/5
        [ResponseType(typeof(ShipperDTO))]
        public async Task<IHttpActionResult> GetShipper(int id)
        {
            Shipper shipper = await db.Shippers.FindAsync(id);
            if (shipper == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Shipper, ShipperDTO>(shipper);
            return Ok(dto);
        }

        // PUT: api/Shippers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutShipper(int id, Shipper shipper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipper.ShipperID)
            {
                return BadRequest();
            }

            db.Entry(shipper).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipperExists(id))
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

        // POST: api/Shippers
        [ResponseType(typeof(Shipper))]
        public async Task<IHttpActionResult> PostShipper(Shipper shipper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shippers.Add(shipper);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = shipper.ShipperID }, shipper);
        }

        // DELETE: api/Shippers/5
        [ResponseType(typeof(Shipper))]
        public async Task<IHttpActionResult> DeleteShipper(int id)
        {
            Shipper shipper = await db.Shippers.FindAsync(id);
            if (shipper == null)
            {
                return NotFound();
            }

            db.Shippers.Remove(shipper);
            await db.SaveChangesAsync();

            return Ok(shipper);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShipperExists(int id)
        {
            return db.Shippers.Count(e => e.ShipperID == id) > 0;
        }
    }
}