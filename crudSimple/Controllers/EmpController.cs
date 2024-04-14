using crudSimple.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using NuGet.Protocol.Core.Types;

namespace crudSimple.Controllers
{
    public class EmpController : Controller
    {
        private readonly IConfiguration _Configuration;
        public EmpController(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }



        [HttpGet]

        public IActionResult getEmpList()
        {
            List<EmpModel> empList = new List<EmpModel>();
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                empList = (List<EmpModel>)con.Query<EmpModel>("SPgetAllEmployees", commandType: CommandType.StoredProcedure);

            }
            return View(empList);
        }

        [HttpGet]
        public IActionResult getEmpByID()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Details(int id)
        {
            List<EmpModel> empList = new List<EmpModel>();
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("EmpID", id);
                empList = (List<EmpModel>)con.Query<EmpModel>("SPgetEmpByID", param, commandType: CommandType.StoredProcedure);

            }
            return View(empList);
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("EmpID", id);
                con.Execute("SPemployeeDelete", param, commandType: CommandType.StoredProcedure);

            }
            return RedirectToAction("getEmpList");
        }



        [HttpGet]
        public IActionResult insertEmployee()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult insertEmployee(EmpModel eMod)
        {
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                var param = new
                {

                    EmpName = eMod.EmpName,
                    EmpEmail = eMod.EmpEmail,
                    EmpSalary = eMod.EmpSalary,
                };
                con.Execute("SPinsertEmp", param, commandType: CommandType.StoredProcedure);
            }
            ModelState.Clear();
            return View();
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            List<EmpModel> empList = new List<EmpModel>();
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("EmpID", id);
                EmpModel em = new EmpModel();
                using (SqlCommand command = new SqlCommand("SPgetEmpByID", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpID", id);

                    // logic to read the data of the entered email id user and store it in the object of the model class
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            em.EmpID = (int)reader["EmpID"];
                            em.EmpName = reader["EmpName"].ToString();
                            em.EmpEmail = reader["EmpEmail"].ToString();
                            em.EmpSalary = (int)reader["EmpSalary"];
                        }
                    }

                }
                return View(em);
            }
        }

        [HttpPost]
        public IActionResult Update(EmpModel eMod)
        {
            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                DynamicParameters dp = new DynamicParameters();
                dp.Add("EmpID", eMod.EmpID);
                dp.Add("EmpName", eMod.EmpName);
                dp.Add("EmpEmail", eMod.EmpEmail);
                dp.Add("EmpSalary", eMod.EmpSalary);
                con.Execute("SPUpdateEmployee", dp, commandType: CommandType.StoredProcedure);
            }
            return RedirectToAction("getEmpList");
        }


        [HttpPost]
        public IActionResult InsertEmployee2(string EmpName, string EmpEmail, decimal EmpSalary)
        {

            using (SqlConnection con = new SqlConnection(_Configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                var param = new
                {

                    EmpName = EmpName,
                    EmpEmail = EmpEmail,
                    EmpSalary = EmpSalary,
                };
                con.Execute("SPinsertEmp", param, commandType: CommandType.StoredProcedure);
            }
            ModelState.Clear();
            return RedirectToAction("getEmpList");
        }
    }
}

