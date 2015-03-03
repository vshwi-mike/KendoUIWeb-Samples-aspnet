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
    public class EmployeesController : ApiController
    {
        private NorthwindDbContext db = new NorthwindDbContext();

        public EmployeesController() {
            Mapper.CreateMap<Employee, EmployeeDTO>();
            Mapper.AssertConfigurationIsValid();
        }

        // GET: api/Employees
        public IEnumerable<EmployeeDTO> GetEmployees(int RegionID = 0, string TerritoryID = "")
        {
            var query = db.Employees
                            .Include("Territories")
                            .AsQueryable();

            if (RegionID != 0) {
                query = query.Where(i => i.Territories.Any(t => t.RegionID == RegionID));
            }

            if (!string.IsNullOrEmpty(TerritoryID)) {
                query = query.Where(i => i.Territories.Any(t => t.TerritoryID == TerritoryID));
            }

            query = query.OrderBy(i => i.EmployeeID);
            var list = Mapper.Map<List<Employee>, List<EmployeeDTO>>(query.ToList());
            return list;
        }

        // GET: api/Employees/5
        [ResponseType(typeof(EmployeeDTO))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }

            var dto = Mapper.Map<Employee, EmployeeDTO>(employee);
            return Ok(dto);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}