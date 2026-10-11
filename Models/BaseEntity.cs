using System;

namespace UniversityPortal.Models
{
    /// <summary>
    /// Абстрактный базовый класс для всех сущностей.
    /// Содержит общие свойства Id, CreatedAt и контракт PrintInfo().
    /// </summary>
    public abstract class BaseEntity
    {
        public int Id { get; set; }
public DateTime CreatedAt { get; private set; }

// ★ PROTECTED — доступно только наследникам (Student, Teacher)
protected string CreatedBy { get; set; } = "System";

/// <summary>
/// Protected метод — наследники могут вызывать, извне — нет.
/// </summary>
protected string GetCreationInfo()
{
    return $"{CreatedBy} • {CreatedAt:dd.MM.yyyy}";
}

        protected BaseEntity(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }

        /// <summary>Абстрактный метод — каждый наследник обязан его реализовать.</summary>
        public abstract void PrintInfo();

        /// <summary>Виртуальный метод — можно переопределить, но не обязательно.</summary>
        public virtual string GetShortInfo()
        {
            return $"[{GetType().Name} #{Id}] создано: {CreatedAt:dd.MM.yyyy HH:mm}";
        }
    }
}