using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SPAPasteMix.Services;
using SPAPasteMix.Services.Formatters;

namespace SPAPasteMix.Controllers
{
    [Produces("application/json")]
    public class PostController : Controller
    {
        public readonly INoteRepository _noteRepository;
        public readonly Dictionary<string,int> _timeoutLimits = new Dictionary<string, int>
        {
            {"", -1},
            { "10m", 10},
            { "1h", 60},
            { "1d", 24*60},
            { "1n", 7*24*60},
            { "1M", 30*24*60}
        };

        public PostController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        public IEnumerable<object> GetPosts()
        {
            return _noteRepository
                .GetNotes()
                .Select(n => new
                {
                    Create = n.Create.ToString(Formatter.GetDateFormat())
                    , Uuid = n.Uuid
                    , Expired = n.Expired?.ToString(Formatter.GetDateFormat())
                    , Title = n.Title
                });
        }

        [HttpGet]
        public object GetPost(string id)
        {
            try
            {
                var note = _noteRepository.GetNote(Guid.Parse(id));
                if (note != null)
                {
                    return new {
                        Create = note.Create.ToString(Formatter.GetDateFormat())
                        ,Expired = note.Expired?.ToString(Formatter.GetDateFormat())
                        ,Title = note.Title
                        ,Text = note.Text
                    };
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPut]
        public object CreatePost(
              [FromQuery(Name = "Title")]string title
            , [FromQuery(Name = "Text")]string text
            , [FromQuery(Name = "TimeLimit")]string timeLimit)
        {
            if (string.IsNullOrEmpty(title))
                return new { error = "Field [title] is empty!"};

            if (string.IsNullOrEmpty(text))
                return new { error = "Field [text] is empty!" };

            if (!TryParseTimeLimit(timeLimit, out DateTime? expired))
            {
                return new { error = $"Expire interval is not valid. Valid values: [{string.Join(",", _timeoutLimits.Keys)}]" };
            }

            _noteRepository.AddNote(new Note
            {
                Create = DateTime.Now,
                Text = text,
                Title = title,
                Expired = expired,
                Uuid = Guid.NewGuid()
            });

            return new {error = ""};
        }

        private bool TryParseTimeLimit(string timeLimit, out DateTime? expired)
        {
            expired = null;
            if (_timeoutLimits.TryGetValue(timeLimit, out int expiredValue))
            {
                if (expiredValue != -1)
                    expired = DateTime.Now.AddMinutes(expiredValue);

                return true;
            }

            return false;
        }
    }
}
