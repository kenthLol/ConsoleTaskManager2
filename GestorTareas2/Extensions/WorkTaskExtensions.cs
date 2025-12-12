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
    }
}
