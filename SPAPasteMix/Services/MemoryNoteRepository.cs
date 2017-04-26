using System;
using System.Collections.Generic;
using System.Linq;

namespace SPAPasteMix.Services
{

    /// <summary>
    /// Простейший тип хранения заметок в памяти.
    /// Это хранилище сделано максимально простым и может сломаться при паралельном доступе со множества потокв.
    /// Также, данное хранилище не предназначено для оперирования сотнями тысяч документов, так как храниться в виде вектора
    /// Удаление элементов с истечением срока хранения сделано при помощи фильтрации
    /// </summary>
    public class MemoryNoteRepository :INoteRepository
    {
        private readonly List<Note> _storage = new List<Note>();

        public MemoryNoteRepository()
        {
        }

        /// <summary>
        /// Метод возвразщает список всех активных элементов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetNotes()
        {
            return _storage
                .Where(i => !i.Expired.HasValue || i.Expired > DateTime.Now)
                .OrderBy(i => i.Create)
                .ToList();
        }

        /// <summary>
        /// Метод сохраняет заметку в хранилище
        /// </summary>
        /// <param name="note"></param>
        public void AddNote(Note note)
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));

            if (string.IsNullOrEmpty(note.Text))
                throw new ArgumentNullException("Text");

            if (string.IsNullOrEmpty(note.Title))
                throw new ArgumentNullException("Title");

            if (note.Uuid != Guid.Empty)
            {
                //проверяем на повторяющиеся Uuid
                if (_storage
                        .Where(i => !i.Expired.HasValue || i.Expired > DateTime.Now)
                        .FirstOrDefault(i => i.Uuid == note.Uuid) != null)
                    throw new InvalidOperationException($"Item with Uuid:{note.Uuid} already exists.");
            }
            else
                note.Uuid = Guid.NewGuid();

            _storage.Add(note);
        }

        /// <summary>
        /// Метод, возвращающий заметку по её идентификатору. В случае просроченной заметки возвращается Null
        /// </summary>
        /// <param name="uuid">Идентификатор заметки</param>
        /// <returns></returns>
        public Note GetNote(Guid uuid)
        {
            return _storage
                .Where(i => !i.Expired.HasValue || i.Expired > DateTime.Now)
                .FirstOrDefault(i => i.Uuid == uuid);
        }
    }
}