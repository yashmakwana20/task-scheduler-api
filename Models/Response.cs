using System.Data;

namespace TaskManagement.Models
{
    public class Response
    {
        public bool IsError { get; set; } = false;

        public DataTable? Data { get; set; }
        //public List<T>? lstData { get; set; }

        public Object? DataModel { get; set; }

        public string? Message { get; set; }

        public int masterId { get; set; }
        public int Count { get; set; }
    }
}
