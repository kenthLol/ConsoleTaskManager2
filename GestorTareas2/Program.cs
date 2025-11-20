using GestorTareas2.Models;

using GestorTareas2.Repositories;
using GestorTareas2.Interfaces;
using GestorTareas2.Services;
using System.Reflection;

string filePath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
IWorkTaskRepository workTaskRepository = new JsonTaskRepository(filePath);

TaskManager taskManager = new TaskManager(workTaskRepository);

while (true)
{
    Console.WriteLine("\n--- GESTOR DE TAREAS ---");
    Console.WriteLine("1. Agregar tarea");
    Console.WriteLine("2. Listar tareas");
    Console.WriteLine("3. Completar tarea");
    Console.WriteLine("4. Eliminar tarea");
    Console.WriteLine("5. Buscar tarea");
    Console.WriteLine("6. Filtrar tareas por prioridad");
    Console.WriteLine("7. Salir");
    Console.Write("Selecciona una opción: ");

    string? input = Console.ReadLine();

    Console.WriteLine();

    switch (input)
    {
        case "1":
            Console.Write("Titulo: ");
            string? title = Console.ReadLine();

            Console.Write("Descripcion: ");
            string? description = Console.ReadLine();

            Console.Write("Prioridad (1-Baja, 2-Media, 3-Alta): ");
            string? priorityStr = Console.ReadLine();

            if (!int.TryParse(priorityStr, out int priorityInput) || !Enum.IsDefined(typeof(Priority), priorityInput))
            {
                priorityInput = (int)Priority.Media;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description))
            {
                taskManager.AddWorkTask(title, description, (Priority)priorityInput);
                Console.WriteLine("Tarea asignada correctamente.");
            }
            else
            {
                Console.WriteLine("El titulo y la descripcion no pueden estar vacios");
            }

            taskManager.CleanScreen();
            break;
        case "2":
            List<WorkTask> results = taskManager.WorkTaskList();

            if (results.Count == 0)
            {
                Console.WriteLine("No hay tareas registradas");
            }
            else
            {
                foreach (WorkTask task in results)
                {
                    string state = task.IsCompleted ? "Completada" : "Pendiente";
                    Console.WriteLine($"{task.Id}. {task.Title} - {state}");
                }
            }

            taskManager.CleanScreen();
            break;
        case "3":
            Console.WriteLine("Id de la tarea a completar: ");

            if (int.TryParse(Console.ReadLine(), out int idCompletar))
            {
                if(!taskManager.CompleteWorkTask(idCompletar))
                {
                    Console.WriteLine("Tarea no encontrada");
                }
                else
                {
                    Console.WriteLine("Tarea completada");
                }
            }

            taskManager.CleanScreen();
            break;
        case "4":
            Console.WriteLine("Id de la tarea a eliminar: ");

            if (int.TryParse(Console.ReadLine(), out int idEliminar))
            {
                if(!taskManager.DeleteWorkTask(idEliminar))
                {
                    Console.WriteLine("Tarea no encontrada");
                }
                else
                {
                    Console.WriteLine("Tarea eliminada exitosamente");
                }
            }

            taskManager.CleanScreen();
            break;
        case "5":
            Console.WriteLine("Buscar (Titulo o Descripcion): ");
            string text = Console.ReadLine() ?? string.Empty;

            if(string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Debe de ingresar un texto válido");
                taskManager.CleanScreen();
                break;
            }

            List<WorkTask> workTaskFound = taskManager.SearchWorkTask(text);

            if (workTaskFound.Count == 0)
            {
                Console.WriteLine("No se encontraron tareas que coincidan con el texto proporcionado");
            }
            else
            {
                foreach(WorkTask task in workTaskFound)
                {
                    string state = task.IsCompleted ? "Completada" : "Pendiente";
                    Console.WriteLine($"{task.Id}. {task.Title} - {state}");
                }
            }

            taskManager.CleanScreen();
            break;

        case "6":
            Console.WriteLine("Buscar por Prioridad (1.Baja, 2.Media, 3.Alta): ");
            string? priorityStrSort = Console.ReadLine();

            if (!int.TryParse(priorityStrSort, out int priority) || !Enum.IsDefined(typeof(Priority), priority))
            {
                priority = (int)Priority.Media;
            }

            List<WorkTask> taskWithPriorityFound = taskManager.FilterWorkTasksByPriority((Priority)priority);

            if(taskWithPriorityFound.Count == 0)
            {
                Console.WriteLine("No hay tareas con esa prioridad");
            }
            else
            {
                foreach(WorkTask task in taskWithPriorityFound)
                {
                    Console.WriteLine($"{task.Id} - {task.Title} - {task.Priority}");
                }
            }

                taskManager.CleanScreen();
            break;
        case "7":
            Console.WriteLine("Hasta luego!");

            taskManager.CleanScreen();
            return;
        default:
            Console.WriteLine("Opción inválida.");

            taskManager.CleanScreen();
            break;

    }
}