using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareas.Models;

public class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Media;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public bool IsCompleted { get; set; } = false;
}