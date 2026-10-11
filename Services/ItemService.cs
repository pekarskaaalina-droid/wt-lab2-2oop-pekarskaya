using System;
using System.Collections.Generic;
using System.Linq;
using UniversityPortal.Interfaces;
using UniversityPortal.Models;

namespace UniversityPortal.Services
{
    /// <summary>
    /// Универсальная реализация IItemService<T>.
    /// Работает для любой сущности, наследующей BaseEntity.
    /// </summary>
    public class ItemService<T> : IItemService<T> where T : BaseEntity
    {
        private readonly List<T> _items = new();

        // ================================================
        // ПЕРВЫЙ метод Add — обычный (один аргумент)
        // ================================================
        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        // ================================================
        // ВТОРОЙ метод Add — ПЕРЕГРУЗКА (два аргумента)
        // Именно этого метода не хватало!
        // ================================================
        public void Add(T item, string comment)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
            Console.WriteLine($"  (добавлено с комментарием: {comment})");
        }

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetAll()
        {
            return new List<T>(_items);
        }

        public bool Update(T item)
        {
            if (item == null) return false;
            int index = _items.FindIndex(x => x.Id == item.Id);
            if (index < 0) return false;
            _items[index] = item;
            return true;
        }

        public bool Delete(int id)
        {
            int removed = _items.RemoveAll(x => x.Id == id);
            return removed > 0;
        }

        public int Count => _items.Count;

        public void PrintAll()
        {
            Console.WriteLine($"  В сервисе {Count} записей:");
            foreach (var item in _items)
            {
                item.PrintInfo();
            }
        }
    }
}