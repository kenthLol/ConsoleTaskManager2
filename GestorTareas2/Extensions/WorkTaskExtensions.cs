using GestorTareas2.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace GestorTareas2.Extensions
{
    static class WorkTaskExtensions
    {
        public static IEnumerable<WorkTask> Pending(this IEnumerable<WorkTask> tasks)
        {
            return tasks.Where(t => !t.IsCompleted);
        }

        public static IEnumerable<WorkTask> Search(this IEnumerable<WorkTask> tasks, string text)
        {
            return tasks.Where(wt =>
                    (wt.Title?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (wt.Description?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false)
            );
        }

        public static IEnumerable<WorkTask> ByPriority(this IEnumerable<WorkTask> tasks, Priority priority)
        {
            return tasks.Where(t => t.Priority == priority);
        }

        public static IEnumerable<WorkTask> Latest(this IEnumerable<WorkTask> tasks, int count)
        {
            return tasks.Take(count);
        }
    }
}
