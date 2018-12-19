using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiAngular.Controllers;
using WebApiAngular.Models;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;
using System.Net;



namespace WebApiAngular.Tests.Controllers
{
    [TestClass]
    public class ValuesController : ApiController
    {
       // private ProjectMgmtEntities db = new ProjectMgmtEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
        //string con = 
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new TasksController();
            var result = controller.GetTasks() as List<Task>;
            var testResult = getTask() as List<Task>; ;
            Assert.AreEqual(result.Count, testResult.Count);
        }


        [TestMethod]
        public void TestMethod2(int id)
        {
            id = 2;
            var controller = new TasksController();
            var result = controller.GetTask(id) as Task;
            var testResult = GetTaskById(id) as Task;
            Assert.AreEqual(result.Task1, testResult.Task1);
        }

        public void DeleteReturnsOk()
        {
            int id = 3;
            var testResult = Delete(id) as List<Task>;
            var controller = new TasksController();

            // Act
            IHttpActionResult actionResult = controller.DeleteTask(10);
           // Assert.IsInstanceOfType(actionResult,typeof(testResult));
            // Assert
           // Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }


        public List<Task> getTask()
        {
            List<Task> taskLst = new List<Task>();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select * from  [dbo].[Task]", con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dadapter = new SqlDataAdapter(cmd);
            dadapter.Fill(ds);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.ExecuteNonQuery();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    Task objTask;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objTask = new Task();
                        objTask.Task1 = ds.Tables[0].Rows[i]["Task"].ToString();
                        objTask.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                        objTask.Project_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Project_ID"].ToString());
                        objTask.Parent_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Parent_ID"].ToString());
                        objTask.Start_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Start_Date"].ToString());
                        objTask.End_Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["End_Date"].ToString());
                        objTask.Priority = Convert.ToInt32(ds.Tables[0].Rows[i]["Priority"].ToString());
                        objTask.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                        objTask.UserId = Convert.ToInt32(ds.Tables[0].Rows[i]["UserId"].ToString());
                        taskLst.Add(objTask);
                    }
                }
            }
            return taskLst;
        }

        // GET api/values/5
        public Task GetTaskById(int id)
        {
            List<Task> taskLst = new List<Task>();
            taskLst = getTask();
            Task task = taskLst.Find((p) => p.Task_ID == id);
            return task;
        }

        // POST api/values
        public void Post([FromBody]Task objTask)
        {
            string status = string.Empty;
            if (ModelState.IsValid)
            {
                string Status = string.Empty;
                SqlCommand cmd = new SqlCommand("dbo.AddTask", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "dbo.AddCustomer";
                cmd.Parameters.Add(new SqlParameter("@Task_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Project_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Parent_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Start_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Start_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@End_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar, 100));
                cmd.Parameters[0].Value = objTask.Task_ID;
                cmd.Parameters[1].Value = objTask.Project_ID;
                cmd.Parameters[2].Value = objTask.Parent_ID;
                cmd.Parameters[3].Value = objTask.ProjectName;
                cmd.Parameters[4].Value = objTask.Start_Date;
                cmd.Parameters[5].Value = objTask.End_Date;
                cmd.Parameters[6].Value = objTask.Priority;
                cmd.Parameters[7].Value = objTask.Status;

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Status = objTask.Task1 + " added successfully";
                }
                else
                {
                    Status = objTask.Task1 + " could not be added";
                }
                con.Close();
            }

        }

        // PUT api/values/5
        public void Put(string id, [FromBody]Task objTask)
        {
            string status = string.Empty;
            if (ModelState.IsValid)
            {
                string Status = string.Empty;
                SqlCommand cmd = new SqlCommand("dbo.UpdateTask", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Task_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Project_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Parent_ID", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@ProjectName", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Start_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Start_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@End_Date", SqlDbType.VarChar, 100));
                cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 10));
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar, 100));
                cmd.Parameters[0].Value = objTask.Task_ID;
                cmd.Parameters[1].Value = objTask.Project_ID;
                cmd.Parameters[2].Value = objTask.Parent_ID;
                cmd.Parameters[3].Value = objTask.ProjectName;
                cmd.Parameters[4].Value = objTask.Start_Date;
                cmd.Parameters[5].Value = objTask.End_Date;
                cmd.Parameters[6].Value = objTask.Priority;
                cmd.Parameters[7].Value = objTask.Status;

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Status = objTask.Task_ID + " udpated successfully";
                }
                else
                {
                    Status = objTask.Task_ID + " could not be updated";
                }
                con.Close();
            }
        }

        // DELETE api/values/5
        public List<Task> Delete(int TaskId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[deleetTask]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Task_ID", SqlDbType.Int, 10));
                cmd.Parameters[0].Value = TaskId;
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                con.Close();
              
            }
            catch
            {
                con.Close();
            }
            return getTask();
        }
    }
}

