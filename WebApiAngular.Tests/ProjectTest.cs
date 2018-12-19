using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;
using System.Net;
using WebApiAngular.Models;
using WebApiAngular.Controllers;
using System.Data.SqlClient;
using System.Data;

namespace WebApiAngular.Tests
{

    /// <summary>
    /// Summary description for ProjectTest
    /// </summary>
    [TestClass]
    public class ProjectTest
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
        public ProjectTest()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var controller = new ProjectsController();
            var result = controller.GetProjects() as List<Project>;
            var testResult = getProject() as List<Project>; ;
            Assert.AreEqual(result.Count, testResult.Count);
            //
            // TODO: Add test logic here
            //
        }


        public List<Project> getProject()
        {
            List<Project> projLst = new List<Project>();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("select * from  [dbo].[Project]", con);
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
                    Project objProj;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objProj = new Project();
                        objProj.Project_ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Project_ID"].ToString());
                        objProj.ProjectName = ds.Tables[0].Rows[i]["ProjectName"].ToString();
                        objProj.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"].ToString());
                        objProj.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                        string strManagerId = "0";
                        if (ds.Tables[0].Rows[i]["ManagerId"].ToString() != string.Empty && ds.Tables[0].Rows[i]["ManagerId"].ToString() != null)
                            strManagerId = ds.Tables[0].Rows[i]["ManagerId"].ToString();                       
                        objProj.ManagerId = Convert.ToInt32(strManagerId);
                        objProj.ManagerName = ds.Tables[0].Rows[i]["ManagerName"].ToString();
                        projLst.Add(objProj);
                    }
                }
            }
            return projLst;
        }

    }
}
