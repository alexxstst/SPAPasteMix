using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPAPasteMix.Controllers;
using SPAPasteMix.Services;

namespace SPAPasteMix.UnitTest
{

    [TestClass]
    public class UnitTest_ApiController
    {
        [TestMethod]
        public void Test_GetNotes()
        {
            var noteRepository = new MemoryNoteRepository();

            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = null,
                Text = "Some text 1",
                Title = "Title 1"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now,
                Text = "Some text 2",
                Title = "Title 2"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(100),
                Text = "Some text 3",
                Title = "Title 3"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(1000),
                Text = "Some text 4",
                Title = "Title 4"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(200),
                Text = "Some text 5",
                Title = "Title 5"
            });

            var postController = new PostController(noteRepository);

            var listOfNotes = postController.GetPosts().ToList();
            Assert.AreEqual(4, listOfNotes.Count);

        }

        [TestMethod]
        public void Test_GetNote()
        {
            var noteRepository = new MemoryNoteRepository();
            var guid = Guid.NewGuid();

            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = null,
                Text = "Some text 1",
                Title = "Title 1"
            });
            noteRepository.AddNote(new Note
            {
                Uuid = guid,
                Create = DateTime.Now,
                Expired = DateTime.Now,
                Text = "Some text 2",
                Title = "Title 2"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(100),
                Text = "Some text 3",
                Title = "Title 3"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(1000),
                Text = "Some text 4",
                Title = "Title 4"
            });
            noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Expired = DateTime.Now.AddMinutes(200),
                Text = "Some text 5",
                Title = "Title 5"
            });

            var postController = new PostController(noteRepository);

            var listOfNotes = postController.GetPosts().ToList();
            Assert.AreEqual(4, listOfNotes.Count);

            dynamic value = listOfNotes[0];
            var propValue = (Guid) value.GetType().GetProperty("Uuid").GetValue(value, null);
            Assert.AreEqual(propValue, noteRepository.GetNote(propValue).Uuid);

            value = listOfNotes[1];
            propValue = (Guid) value.GetType().GetProperty("Uuid").GetValue(value, null);
            Assert.AreEqual(propValue, noteRepository.GetNote(propValue).Uuid);

            value = listOfNotes[2];
            propValue = (Guid) value.GetType().GetProperty("Uuid").GetValue(value, null);
            Assert.AreEqual(propValue, noteRepository.GetNote(propValue).Uuid);

            value = listOfNotes[3];
            propValue = (Guid) value.GetType().GetProperty("Uuid").GetValue(value, null);
            Assert.AreEqual(propValue, noteRepository.GetNote(propValue).Uuid);
            Assert.AreEqual(null, noteRepository.GetNote(guid));
        }

        [TestMethod]
        public void Test_AddNote()
        {
            var noteRepository = new MemoryNoteRepository();
            var postController = new PostController(noteRepository);

            string title = "Title 1";
            string text = "Title 1";
            string limit = "10m";
            postController.CreatePost(title, text, limit);

            var note = noteRepository.GetNotes().ToList()[0];
            Assert.AreEqual(title, note.Title);
            Assert.AreEqual(text, note.Text);
            Assert.AreEqual(10, Convert.ToInt32((note.Expired - note.Create).Value.TotalMinutes));
        }
    }

}