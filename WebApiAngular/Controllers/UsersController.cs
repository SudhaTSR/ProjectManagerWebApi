using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiAngular.Models;
using System.Web.Http.Cors;

namespace WebApiAngular.Controllers
{
    public class UsersController : ApiController
    {
        private ProjectMgmtEntities db = new ProjectMgmtEntities();

        // GET: api/Users
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<User> GetUsers()
        {

            var users = db.Users.ToList();
            var response = new List<User>();

            foreach (var obj in users)
            {
                response.Add(new User()
                {
                    User_ID = obj.User_ID,
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    Employee_Id = obj.Employee_Id,
                    Project_ID = obj.Project_ID,
                    Task_ID = obj.Task_ID,                  
                    // all other properties

                });
            }
            return response;
        }

        // GET: api/Users/5        
        [ResponseType(typeof(User))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.User_ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.User_ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.User_ID == id) > 0;
        }
    }
}