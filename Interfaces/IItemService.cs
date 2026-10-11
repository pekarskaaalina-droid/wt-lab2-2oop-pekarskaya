using System;
using System.Collections.Generic;

namespace UniversityPortal.Interfaces
{
    /// <summary>
    /// Обобщённый интерфейс сервиса управления сущностями.
    /// T — тип сущности (Student, Teacher и т.д.).
    /// </summary>
    public interface IItemService<T>
    {
        // --- CRUD-контракты ---
        void Add(T item);

        // ★ ПЕРЕГРУЗКА — Add с комментарием (для демонстрации method overloading)
        void Add(T item, string comment);

        T? GetById(int id);
        List<T> GetAll();
        bool Update(T item);
        bool Delete(int id);

        // --- Дополнительные контракты ---
        int Count { get; }
        void PrintAll();
    }
}