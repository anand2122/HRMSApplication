using HRMSApplication.Core.Models;
using HRMSApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly DepartmentService _deptService;
        private readonly IConfiguration _configuration;

        private readonly IEmployeeBizLayer _bizLayer;
        public EmployeeController(IEmployeeBizLayer bizLayer, DepartmentService service, IConfiguration configuration)
        {
            _bizLayer = bizLayer;
            _deptService = service;
            _configuration = configuration;
        }

        [HttpGet("validate")] 
        public IActionResult ValidateToken() 
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty); 
            var key = _configuration["Jwt:Key"]; 
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var principal = JwtTokenHelper.ValidateToken(token, key, issuer, audience); 
            if (principal != null)
            {
                var username = principal.Identity.Name; 
                return Ok(new { Username = username });
            } 
            return Unauthorized(); 
        }
        // GET: People
        public async Task<IActionResult> Employee()
        {
            return View(await _bizLayer.GetEmployeeAsync());
        }


        public async Task<IActionResult> Employee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var person = await _bizLayer.GetEmployeeAsync(id.Value);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Designation,DepartmentNo,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                SetEmployeeSalary(employee);

                // check department exist or not in cosmos db
                var empDeptExist = _deptService.DepartmentExistsAsync(employee.DepartmentNo);

                if (empDeptExist.IsCompleted)
                {
                    await _bizLayer.AddEmployeeAsync(employee);

                    return RedirectToAction(nameof(Employee));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Inputed department :" + employee.DepartmentNo + "doen't exist");
                }

            }
            return View(employee);
        }

        private static void SetEmployeeSalary(Employee employee)
        {
            employee.Salary = employee.Designation switch
            {
                "Jr. Developer" => 40000,
                "Developer" => 50000,
                "Sr. Developer" => 70000,
                "Team Lead" => 100000,
                "Project Lead" => 150000,
                "Project Manager" => 220000,
                _ => 0
            };
        }

        // GET: People/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var person = await _context.People.FindAsync(id);
            var employee = await _bizLayer.GetEmployeeAsync(id.Value);

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update salary based on designation
                    SetEmployeeSalary(employee);

                    await _bizLayer.ModifyEmployeeAsync(employee);
                }
                catch (Exception)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Employee));
            }
            return View(employee);
        }

        // GET: People/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _bizLayer.GetEmployeeAsync(id.Value);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bizLayer.RemoveEmployeeAsync(id);

            return RedirectToAction(nameof(Employee));
        }

        private bool EmployeeExists(int id)
        {
            return _bizLayer.GetEmployeeAsync(id) != null;
        }
    }
}

