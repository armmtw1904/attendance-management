using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceManagement.Models
{
    public class AttendanceModel
    {
        public int SubjectId { get; set; }
        public List<Session> SessionList { get; set; }
        public List<Student> StudentList { get; set; }
    }
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Student
    {
        public int RollNo { get; set; }
        public string FullName { get; set; }
        public bool Abscent { get; set; }
    }
    public class AttendanceSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RollNo { get; set; }
        public string FullName { get; set; }
    }
}
