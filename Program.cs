using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityPortal.Models;
using UniversityPortal.Interfaces;
using UniversityPortal.Services;

namespace UniversityPortal
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ShowWelcome();
            DemonstrateBasicClass();
            DemonstratePolymorphism();
            DemonstrateService();
            DemonstrateCollections();      // ← Блок 2, Уровень 1
            DemonstrateDictionary();       // ← Блок 2, Уровень 2
            DemonstrateLinq();             // ← Блок 2, Уровень 3

            // === Блок 3: Асинхронное программирование ===
            await DemonstrateAsyncBasic();           // ← Уровень 1
            await DemonstrateAsyncWithReturn();      // ← Уровень 2
            await DemonstrateParallelAndCancel();    // ← Уровень 3

            Console.WriteLine();
            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        static void ShowWelcome()
        {
            Console.WriteLine();
            Console.WriteLine("  УНИВЕРСИТЕТ / СТУДЕНЧЕСКИЙ ПОРТАЛ");
            Console.WriteLine("  Лабораторная работа №2 ");
            Console.WriteLine("  Автор: Пекарская Алина");
            Console.WriteLine();
        }

        static void DemonstrateBasicClass()
        {
            Console.WriteLine("Класс Student");
            Console.WriteLine();

            Student s1 = new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75);

            Student s2 = new Student
            {
                Id = 2,
                FullName = "Остапчук Фиолента Андреевна",
                Group = "УИР-2"
            };
            s2.UpdateGrade(9.20);

            Student s3 = new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0);

            s1.PrintInfo();
            Console.WriteLine();
            s2.PrintInfo();
            Console.WriteLine();
            s3.PrintInfo();
            Console.WriteLine();
        }

        static void DemonstratePolymorphism()
        {
            Console.WriteLine("Наследование и полиморфизм ");
            Console.WriteLine();

            List<BaseEntity> entities = new List<BaseEntity>
            {
                new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75),
                new Teacher(2, "Шаститко Дмитрий Витальевич", "Кафедра УИР", "Ст. преподаватель"),
                new Student(3, "Остапчук Фиолента Андреевна", "УИР-2", 9.20),
                new Teacher(4, "Юрча Ирина Александровна", "Кафедра УИР", "Ст. преподаватель")
            };

            Console.WriteLine("Полиморфный вызов PrintInfo():");
            foreach (BaseEntity e in entities)
            {
                e.PrintInfo();
                Console.WriteLine();
            }

            Console.WriteLine("GetShortInfo() через базовую ссылку:");
            foreach (BaseEntity e in entities)
            {
                Console.WriteLine("  " + e.GetShortInfo());
            }

            Console.WriteLine();
            Console.WriteLine("Инкапсуляция: изменение AverageGrade через UpdateGrade():");
            Student s = new Student(5, "Тестов Тест Тестович", "УИР-3", 5.00);
            Console.WriteLine($"  До:    {s.AverageGrade:F2}");
            s.UpdateGrade(9.50);
            Console.WriteLine($"  После: {s.AverageGrade:F2}");

            try
            {
                s.UpdateGrade(15);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  Ошибка валидации: {ex.Message}");
            }
        }

        static void DemonstrateService()
        {
            Console.WriteLine();
            Console.WriteLine("Сервис IItemService<T>");
            Console.WriteLine();

            IItemService<Student> studentService = new ItemService<Student>();

            studentService.Add(new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75));
            studentService.Add(new Student(2, "Остапчук Фиолента Андреевна", "УИР-2", 9.20));

            studentService.Add(
                new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0),
                "добавлен через перегрузку метода"
            );

            Console.WriteLine($"Всего в сервисе: {studentService.Count}");
            Console.WriteLine();

            studentService.PrintAll();

            Console.WriteLine();
            var found = studentService.GetById(2);
            if (found != null)
            {
                Console.WriteLine("Поиск по Id=2:");
                found.PrintInfo();
            }

            Console.WriteLine();

            if (found is Student student2)
            {
                Console.WriteLine("  " + student2.GetStudentCreationInfo());
            }

            Console.WriteLine();
            bool deleted = studentService.Delete(1);
            Console.WriteLine($"Удаление Id=1: {(deleted ? "успешно" : "не найдено")}");
            Console.WriteLine($"Осталось в сервисе: {studentService.Count}");
        }

        static void DemonstrateCollections()
        {
            Console.WriteLine();
            Console.WriteLine("Коллекция List<Student>");
            Console.WriteLine();

            List<Student> students = new List<Student>
            {
                new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75),
                new Student(2, "Остапчук Фиолента Андреевна", "УИР-2", 9.20),
                new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0),
                new Student(4, "Винник Диана Денисовна", "УИР-2", 9.50),
                new Student(5, "Аляшевич Милана Витальевна", "УИР-2", 8.90),
                new Student(6, "Пекарская Алина Витальевна", "УИР-2", 9.40)
            };

            Console.WriteLine($"Всего студентов: {students.Count}");
            Console.WriteLine();

            foreach (Student s in students)
            {
                s.PrintInfo();
            }
        }

        static void DemonstrateDictionary()
        {
            Console.WriteLine();
            Console.WriteLine("Словарь Dictionary<int, Student>");
            Console.WriteLine();

            Dictionary<int, Student> studentsById = new Dictionary<int, Student>
            {
                { 1, new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75) },
                { 2, new Student(2, "Остапчук Фиолента Андреевна", "УИР-2", 9.20) },
                { 3, new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0) },
                { 4, new Student(4, "Винник Диана Денисовна", "УИР-2", 9.50) },
                { 5, new Student(5, "Аляшевич Милана Витальевна", "УИР-2", 8.90) }
            };

            Console.WriteLine($"Всего в словаре: {studentsById.Count}");
            Console.WriteLine();

            Console.WriteLine("Поиск Id=3 (существующий):");
            if (studentsById.TryGetValue(3, out Student? found))
            {
                found.PrintInfo();
            }
            else
            {
                Console.WriteLine("  Студент с таким Id не найден.");
            }

            Console.WriteLine();

            Console.WriteLine("Поиск Id=99 (несуществующий):");
            if (studentsById.TryGetValue(99, out Student? notFound))
            {
                notFound.PrintInfo();
            }
            else
            {
                Console.WriteLine("  Студент с Id=99 не найден. Обработка ситуации отсутствия ключа — OK.");
            }

            Console.WriteLine();

            Console.WriteLine("Прямой доступ через индексатор (для Id=2):");
            Student s2 = studentsById[2];
            Console.WriteLine($"  Найден: {s2.FullName}, балл: {s2.AverageGrade:F2}");
        }

        static void DemonstrateLinq()
        {
            Console.WriteLine();
            Console.WriteLine("LINQ — запросы к коллекции студентов");
            Console.WriteLine();

            List<Student> students = new List<Student>
            {
                new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75),
                new Student(2, "Остапчук Фиолента Андреевна", "УИР-2", 9.20),
                new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0),
                new Student(4, "Винник Диана Денисовна", "УИР-3", 9.50),
                new Student(5, "Аляшевич Милана Витальевна", "УИР-2", 8.90),
                new Student(6, "Пекарская Алина Витальевна", "УИР-2", 9.40)
            };

            Console.WriteLine("1. Отличники (балл >= 9.0):");
            var excellent = students.Where(s => s.AverageGrade >= 9.0).ToList();
            foreach (var s in excellent)
            {
                Console.WriteLine($"   {s.FullName} — {s.AverageGrade:F2}");
            }
            Console.WriteLine();

            Console.WriteLine("2. Сортировка по среднему баллу (по убыванию):");
            var sorted = students.OrderByDescending(s => s.AverageGrade).ToList();
            foreach (var s in sorted)
            {
                Console.WriteLine($"   {s.AverageGrade:F2} — {s.FullName}");
            }
            Console.WriteLine();

            Console.WriteLine("3. Проекция: только ФИО студентов:");
            var names = students.Select(s => s.FullName).ToList();
            foreach (var name in names)
            {
                Console.WriteLine($"   {name}");
            }
            Console.WriteLine();

            Console.WriteLine("4. Агрегатные операции:");
            Console.WriteLine($"   Count   = {students.Count}");
            Console.WriteLine($"   Sum     = {students.Sum(s => s.AverageGrade):F2}");
            Console.WriteLine($"   Average = {students.Average(s => s.AverageGrade):F2}");
            Console.WriteLine($"   Min     = {students.Min(s => s.AverageGrade):F2}");
            Console.WriteLine($"   Max     = {students.Max(s => s.AverageGrade):F2}");
            Console.WriteLine();

            Console.WriteLine("5. Группировка по группам:");
            var grouped = students.GroupBy(s => s.Group);
            foreach (var g in grouped)
            {
                Console.WriteLine($"   Группа {g.Key}:");
                Console.WriteLine($"     студентов:   {g.Count()}");
                Console.WriteLine($"     средний балл: {g.Average(s => s.AverageGrade):F2}");
                Console.WriteLine($"     лучший балл:  {g.Max(s => s.AverageGrade):F2}");
            }
        }

        // === Блок 3, Уровень 1: async/await ===
        static async Task DemonstrateAsyncBasic()
        {
            Console.WriteLine();
            Console.WriteLine("Блок 3, Уровень 1: async/await");
            Console.WriteLine();

            Console.WriteLine("  Начинаю длительную операцию...");
            Console.WriteLine($"  Время старта: {DateTime.Now:HH:mm:ss.fff}");

            await Task.Delay(1000);

            Console.WriteLine("  Готово: операция завершена.");
            Console.WriteLine($"  Время конца:  {DateTime.Now:HH:mm:ss.fff}");
        }

        // === Блок 3, Уровень 2: async Task<List<T>> + try-catch-finally ===
        static async Task<List<Student>> LoadStudentsAsync()
        {
            Console.WriteLine("  [Async] Подключение к источнику данных...");
            await Task.Delay(1500);

            Console.WriteLine("  [Async] Данные получены.");
            return new List<Student>
            {
                new Student(1, "Иванов Иван Иванович", "УИР-2", 8.75),
                new Student(2, "Остапчук Фиолента Андреевна", "УИР-2", 9.20),
                new Student(3, "Лисейчиков Денис Олегович", "УИР-2", 10.0),
                new Student(4, "Винник Диана Денисовна", "УИР-2", 9.50)
            };
        }

        static async Task DemonstrateAsyncWithReturn()
        {
            Console.WriteLine();
            Console.WriteLine("Блок 3, Уровень 2: async Task<List<T>> + try-catch-finally");
            Console.WriteLine();

            List<Student>? students = null;

            try
            {
                students = await LoadStudentsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [Catch] Ошибка при загрузке: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("  [Finally] Блок завершён — ресурсы освобождены.");
            }

            if (students != null && students.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"  Загружено студентов: {students.Count}");
                foreach (var s in students)
                {
                    Console.WriteLine($"    {s.FullName} — {s.AverageGrade:F2} ({s.Group})");
                }
            }
        }

        // ==================================================
        // Блок 3, Уровень 3: Task.WhenAll + CancellationToken
        // ==================================================

        /// <summary>
        /// Имитация длительной загрузки кафедры с поддержкой отмены.
        /// </summary>
        static async Task<string> LoadDepartmentAsync(string name, int delayMs, CancellationToken token)
        {
            // Проверяем — не отменили ли задачу до старта
            token.ThrowIfCancellationRequested();

            // Ждём с поддержкой отмены
            await Task.Delay(delayMs, token);

            return $"Кафедра '{name}' загружена (за {delayMs} мс)";
        }

        static async Task DemonstrateParallelAndCancel()
        {
            Console.WriteLine();
            Console.WriteLine("Блок 3, Уровень 3: Task.WhenAll + CancellationToken");
            Console.WriteLine();

            // ============================================
            // Часть A: Task.WhenAll — параллельный запуск
            // ============================================
            Console.WriteLine("A. Параллельная загрузка 3 кафедр (Task.WhenAll):");
            Console.WriteLine($"   Время старта: {DateTime.Now:HH:mm:ss.fff}");

            using CancellationTokenSource cts = new CancellationTokenSource();

            Task<string> t1 = LoadDepartmentAsync("УИР", 1000, cts.Token);
            Task<string> t2 = LoadDepartmentAsync("ИЯ", 1500, cts.Token);
            Task<string> t3 = LoadDepartmentAsync("ФК", 800, cts.Token);

            // Ждём завершения ВСЕХ трёх задач параллельно
            string[] results = await Task.WhenAll(t1, t2, t3);

            Console.WriteLine($"   Время конца:  {DateTime.Now:HH:mm:ss.fff}");
            foreach (var r in results)
            {
                Console.WriteLine($"   - {r}");
            }
            Console.WriteLine("   (общее время ~1.5 сек, а не 3.3 сек — значит выполнялись параллельно)");
            Console.WriteLine();

            // ============================================
            // Часть B: CancellationToken — отмена задачи
            // ============================================
            Console.WriteLine("B. Отмена длительной задачи через CancellationToken:");
            Console.WriteLine("   Запущена задача на 5 секунд, отмена через 0.5 секунды...");

            using CancellationTokenSource cancelSource = new CancellationTokenSource();

            Task<string> longTask = LoadDepartmentAsync("Долгая кафедра", 5000, cancelSource.Token);

            // Отменяем через 500 мс
            cancelSource.CancelAfter(500);

            try
            {
                string r = await longTask;
                Console.WriteLine($"   {r}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("   Задача была отменена через CancellationToken — OK.");
            }
        }
    }
}