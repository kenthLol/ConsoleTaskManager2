using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareas2.Utils
{
    public static class UIHelper
    {
        /// <summary>
        /// Prints each item in the specified collection to the console. If the collection is empty, prints the provided
        /// message instead.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the collection to print.</typeparam>
        /// <param name="items">The collection of items to print to the console. If the collection is empty, no items are printed.</param>
        /// <param name="emptyMessage">The message to print to the console if the collection contains no items. Cannot be null.</param>
        public static void PrintList<T>(IEnumerable<T> items, string emptyMessage)
        {
            var list = items.ToList();

            if(list.Count > 0)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {list[i]}");
                }
            }
            else
            {
                Console.WriteLine(emptyMessage);
            }
        }
    }
}
