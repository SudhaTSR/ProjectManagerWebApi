using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiAngular.Models;
using System.Web.Http.Cors;


namespace WebApiAngular.Controllers
{
    public class TasksController : ApiController
    {
        private ProjectMgmtEntities db = new ProjectMgmtEntities();

        // GET: api/Tasks        
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Task> GetTasks()
        {
            var tasks = db.Tasks.ToList();
            var response = new List<Task>();

            foreach (var obj in tasks)
            {
                response.Add(new Task()
                {
                    Task_ID  = obj.Task_ID,
                    Parent_ID = obj.Parent_ID,
                    Project_ID  = obj.Project_ID,
                    Start_Date  = obj.Start_Date,
                    End_Date  = obj.End_Date,
                    Status  = obj.Status,
                    Priority  = obj.Priority,                    
                    // all other properties
                    
                });
            }
            return response;           
        }
        //public IQueryable<Task> GetTasks()
        //{
        //    return db.Tasks;
        //}

        // GET: api/Tasks/5
        [ResponseType(typeof(Task))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        // PUT: api/Tasks/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutTask(int id, Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != task.Task_ID)
            {
                return BadRequest();
            }

            db.Entry(task).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        [ResponseType(typeof(Task))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostTask(Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tasks.Add(task);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = task.Task_ID }, task);
        }

        // DELETE: api/Tasks/5
        [ResponseType(typeof(Task))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteTask(int id)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();

            return Ok(task);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaskExists(int id)
        {
            return db.Tasks.Count(e => e.Task_ID == id) > 0;
        }
    }
}