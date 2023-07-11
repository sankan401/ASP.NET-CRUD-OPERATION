using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeeController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
     

        [HttpPost]
        public  async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth

            };
            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task< IActionResult >View(Guid id) 
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
                var viewModel = new Update()
                {

                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(()=>View("View",viewModel));
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult >View(Update model)
        {
            var employee =await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Id = model.Id;
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Department = model.Department;
                employee.DateOfBirth = model.DateOfBirth;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Add");

        }

        [HttpPost]
        public async Task<IActionResult> Remove(Update model) 
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null) 
            {
                mvcDemoDbContext.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}
