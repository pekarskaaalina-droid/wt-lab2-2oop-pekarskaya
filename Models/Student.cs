using System;

namespace UniversityPortal.Models
{
    /// <summary>
    /// Студент. Наследуется от BaseEntity.
    /// Демонстрирует ИНКАПСУЛЯЦИЮ: AverageGrade закрыт для прямого изменения.
    /// </summary>
    public class Student : BaseEntity
    {
        // Открытые автосвойства
        public string FullName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;

        // ★ ИНКАПСУЛЯЦИЯ: private set — нельзя изменить напрямую снаружи
        public double AverageGrade { get; private set; }

        public Student(int id, string fullName, string group, double averageGrade)
            : base(id)
        {
            FullName = fullName;
            Group = group;
            // Через метод с валидацией, а не напрямую
            UpdateGrade(averageGrade);
        }

        public Student() : base(0) { }

        /// <summary>
        /// Обновление среднего балла с валидацией (0..10).
        /// </summary>
        public void UpdateGrade(double newGrade)
        {
            if (newGrade < 0 || newGrade > 10)
            {
                throw new ArgumentException("Оценка должна быть в диапазоне 0..10");
            }
            AverageGrade = newGrade;
        }

        // ★ Переопределяем абстрактный метод из BaseEntity
        public override void PrintInfo()
        {
            Console.WriteLine($"Студент #{Id}: {FullName}");
            Console.WriteLine($"  Группа:       {Group}");
            Console.WriteLine($"  Средний балл: {AverageGrade:F2}");
        }

        // ★ Переопределяем виртуальный метод
        public override string GetShortInfo()
        {
            return $"Студент #{Id}: {FullName} ({Group}), балл: {AverageGrade:F2}";
        }

        // ★ Демонстрация protected: наследник Student использует protected-члены родителя BaseEntity
        public string GetStudentCreationInfo()
        {
            CreatedBy = "StudentService";                       // ← protected-свойство BaseEntity
            return $"Студент создан: {GetCreationInfo()}";      // ← protected-метод BaseEntity
        }
    }
}