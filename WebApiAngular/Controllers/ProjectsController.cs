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
using System.Web.UI;
using WebApiAngular.Models;
using System.Web.Http.Cors;

namespace WebApiAngular.Controllers
{
    public class ProjectsController : ApiController
    {
        private ProjectMgmtEntities  db = new ProjectMgmtEntities();

        // GET: api/Projects
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<Project> GetProjects()
        {
            var Projects = db.Projects.ToList();
            var response = new List<Project>();

            foreach (var obj in Projects)
            {
                response.Add(new Project()
                {
                    Project_ID = obj.Project_ID,
                    ProjectName = obj.ProjectName,
                    StartDate = obj.StartDate,
                    EndDate = obj.EndDate,
                    Priority = obj.Priority,
                    ManagerId = obj.ManagerId,
                    ManagerName = obj.ManagerName,
                    // all other properties

                });
            }
            return response;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Project_ID)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.Project_ID }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteProject(int id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.Project_ID == id) > 0;
        }
    }
}