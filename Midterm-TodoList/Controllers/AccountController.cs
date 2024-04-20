using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.OleDb;

namespace TodoListApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Check login logic
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            // Register logic
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("AccessConnection");

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO tblUser (Username, Password) VALUES (?, ?)";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", newUser.Email);
                        command.Parameters.AddWithValue("@Password", newUser.Password);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login");
            }
            else
            {
                // Model validation failed, return the registration view with validation errors
                return View(newUser);
            }
        }
    }
}
