using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPAPasteMix.Services;

namespace SPAPasteMix.UnitTest
{
    [TestClass]
    public class UnitTest_NoteRepository
    {
        [TestMethod]
        public void Test_GetNotes()
        {
            var noteRepository = new MemoryNoteRepository();

            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = null, Text = "Some text 1", Title = "Title 1" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now, Text = "Some text 2", Title = "Title 2" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(100), Text = "Some text 3", Title = "Title 3" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(1000), Text = "Some text 4", Title = "Title 4" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(200), Text = "Some text 5", Title = "Title 5" });

            var listOfNotes = noteRepository.GetNotes().ToList();
            Assert.AreEqual(4, listOfNotes.Count);
        }

        [TestMethod]
        public void Test_GetNote()
        {
            var noteRepository = new MemoryNoteRepository();
            var guid = Guid.NewGuid();

            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = null, Text = "Some text 1", Title = "Title 1" });
            noteRepository.AddNote(new Note { Uuid = guid, Create = DateTime.Now, Expired = DateTime.Now, Text = "Some text 2", Title = "Title 2" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(100), Text = "Some text 3", Title = "Title 3" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(1000), Text = "Some text 4", Title = "Title 4" });
            noteRepository.AddNote(new Note { Create = DateTime.Now, Expired = DateTime.Now.AddMinutes(200), Text = "Some text 5", Title = "Title 5" });

            var listOfNotes = noteRepository.GetNotes().ToList();
            Assert.AreEqual(4, listOfNotes.Count);

            Assert.AreEqual(listOfNotes[0].Uuid, noteRepository.GetNote(listOfNotes[0].Uuid).Uuid);
            Assert.AreEqual(listOfNotes[1].Uuid, noteRepository.GetNote(listOfNotes[1].Uuid).Uuid);
            Assert.AreEqual(listOfNotes[2].Uuid, noteRepository.GetNote(listOfNotes[2].Uuid).Uuid);
            Assert.AreEqual(listOfNotes[3].Uuid, noteRepository.GetNote(listOfNotes[3].Uuid).Uuid);
            Assert.AreEqual(null, noteRepository.GetNote(guid));
        }

        [TestMethod]
        public void Test_AddNote()
        {
            var noteRepository = new MemoryNoteRepository();

            try
            {
                noteRepository.AddNote(new Note
                {
                    Create = DateTime.Now,
                    Expired = null,
                    Text = "",
                    Title = "Title 1"
                });
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Text"));
            }

            try
            {
                noteRepository.AddNote(new Note
                {
                    Create = DateTime.Now,
                    Expired = null,
                    Text = "fgfg",
                    Title = ""
                });
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("Title"));
            }
        }
    }
}
