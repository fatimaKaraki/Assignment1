using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Models
{
    public class TaskToDo 
    {
        public int TaskId { get; set; }
        public string Username {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly StartingDate { get; set; }
        public DateOnly DueDate { get; set; }
        public Status Status { get; set; }
    }

    public enum Status
    {
     
        Incomplete,
        Completed,
        Canceled
    }
}
