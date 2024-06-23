using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentLMS.Data;
using StudentLMS.Models;
using StudentLMS.Models.Entities;


namespace StudentLMS.Controllers
{
    [Route("students")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
          
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed,
            };
            await dbContext.Students.AddAsync(student);
            dbContext.SaveChanges();
            return RedirectToAction("List", "Students");
        }
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            Console.WriteLine("this route is working");
            var students = await dbContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await dbContext.Students.FindAsync(viewModel.Id);
            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
        [HttpPost("delete")]
        public  async Task<IActionResult> Delete(Student viewModel)
        {
            Console.WriteLine("this is delete route");
            var student = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == viewModel.Id);    
            if(student is not null)
            {
                dbContext.Students.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
