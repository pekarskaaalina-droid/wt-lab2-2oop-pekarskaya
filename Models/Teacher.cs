using System;

namespace UniversityPortal.Models
{
    /// <summary>
    /// Преподаватель. Тоже наследуется от BaseEntity.
    /// </summary>
    public class Teacher : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;

        public Teacher(int id, string fullName, string department, string degree)
            : base(id)
        {
            FullName = fullName;
            Department = department;
            Degree = degree;
        }

        public Teacher() : base(0) { }

        public override void PrintInfo()
        {
            Console.WriteLine($"Преподаватель #{Id}: {FullName}");
            Console.WriteLine($"  Кафедра:       {Department}");
            Console.WriteLine($"  Степень:       {Degree}");
        }

        public override string GetShortInfo()
        {
            return $"Преподаватель #{Id}: {FullName} ({Degree})";
        }
    }
}