using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Dapper;

namespace AttendanceManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _db;

        public HomeController(IService db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            AttendanceModel model = new AttendanceModel();
            var sessions =_db.GetAll<Session>(@"SELECT * FROM Session", null, CommandType.Text).ToList();
            var students =_db.GetAll<Student>(@"SELECT * FROM Student", null, CommandType.Text).ToList();
            students.ForEach(f => f.Abscent = true);
            model.StudentList = students;
            model.SessionList = sessions;
            return View(model);
        }
        [HttpPost]
        public IActionResult MarkAttendance([FromForm] int subject, [FromForm] int[] abscenties)
        {
            if (subject == 0 || !abscenties.Any())
                return Content("invalid input");
            //List<int> students = student.Select(s => Int32.Parse(s)).ToList();
            //var con = _db.GetDbconnection();
            //IEnumerable<int> allStudents = con.Query<int>(@"SELECT RollNo FROM Student", null, commandType: CommandType.Text);
            var parameter = abscenties.Select(s => new
            {
                StudentId = s,
                SessionId = subject,
                Date = DateTime.Now
            }).ToList();

            var res = _db.Execute(@"INSERT INTO AbscentList(StudentId,SessionId,Date) VALUES (@StudentId, @SessionId,@Date)", parameter, CommandType.Text);
            return Content("successfully marked attendence");
        }
        public IActionResult GetAttendanceSummary()
        {
            var con = _db.GetDbconnection();
            var res = con.Query<AttendanceSummary>("spc_attendance_summary", null, commandType: CommandType.StoredProcedure);
            var dist = res.GroupBy(g => new { g.RollNo, g.Id }).Select(s => s.FirstOrDefault());
            var p = dist.GroupBy(g => g.RollNo).Where(s => s.Count() > 3)
                .Select(s => new Student
                {
                    RollNo = s.FirstOrDefault().RollNo,
                    FullName = s.FirstOrDefault().FullName
                }).OrderBy(o=> o.RollNo).ToList();
            return View(p);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
