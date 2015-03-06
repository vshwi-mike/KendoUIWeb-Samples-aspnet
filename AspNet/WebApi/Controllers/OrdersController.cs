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
    public class OrdersController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public OrdersController() {
            Mapper.CreateMap<Customer, CustomerDTO>();
            Mapper.CreateMap<Employee, EmployeeDTO>();
            Mapper.CreateMap<Order, OrderDTO>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.CompanyName : ""))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? s.Employee.FirstName + " " + s.Employee.LastName : ""));
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Orders
        public IEnumerable<OrderDTO> GetOrders(){
            var query = db.Orders
                            .Include("Customer")
                            .Include("Employee")
                            .OrderByDescending(i => i.OrderID);
            var list = Mapper.Map<List<Order>, List<OrderDTO>>(query.ToList());
            return list;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(OrderDTO))]
        public async Task<IHttpActionResult> GetOrder(int id){
            Order order = await db.Orders
                        .Include("Customer")
                        .Include("Employee")
                        .SingleOrDefaultAsync(i => i.OrderID == id);
            if (order == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Order, OrderDTO>(order);
            return Ok(dto);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != order.OrderID) {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!OrderExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PostOrder(Order order) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = order.OrderID }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id) {
            Order order = await db.Orders.FindAsync(id);
            if (order == null) {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id) {
            return db.Orders.Any(e => e.OrderID == id);
        }
    }
}