using System;
using System.Collections.Generic;

namespace SPAPasteMix.Services
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetNotes();
        void AddNote(Note note);
        Note GetNote(Guid uuid);
    }
}