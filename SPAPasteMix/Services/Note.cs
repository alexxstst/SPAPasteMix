using System;

namespace SPAPasteMix.Services
{
    public class Note
    {
        public DateTime Create { get; set; }
        public DateTime? Expired { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid Uuid { get; set; }
    }
}