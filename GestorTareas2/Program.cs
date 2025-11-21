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
            string? priorityValue = Console.ReadLine();

            if (!int.TryParse(priorityValue, out int priority) || !Enum.IsDefined(typeof(Priority), priority))
            {
                priority = (int)Priority.Media;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description))
            {
                taskManager.AddWorkTask(title, description, (Priority)priority);
                Console.WriteLine("Tarea asignada correctamente.");
            }
            else
            {
                Console.WriteLine("El titulo y la descripcion no pueden estar vacios");
            }

            CleanScreen();
            break;
        case "2":
            List<WorkTask> results = taskManager.GetAllWorkTaskList();

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

            CleanScreen();
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

            CleanScreen();
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

            CleanScreen();
            break;
        case "5":
            Console.WriteLine("Buscar (Titulo o Descripcion): ");
            string text = Console.ReadLine() ?? string.Empty;

            if(string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Debe de ingresar un texto válido");
                CleanScreen();
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

            CleanScreen();
            break;

        case "6":
            Console.WriteLine("Buscar por Prioridad (1.Baja, 2.Media, 3.Alta): ");
            string? selectedPriority = Console.ReadLine();

            if (!int.TryParse(selectedPriority, out int priorityOption) || !Enum.IsDefined(typeof(Priority), priorityOption))
            {
                priority = (int)Priority.Media;
            }

            List<WorkTask> taskWithPriorityFound = taskManager.FilterWorkTasksByPriority((Priority)priorityOption);

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

                CleanScreen();
            break;
        case "7":
            Console.WriteLine("Hasta luego!");

            CleanScreen();
            return;
        default:
            Console.WriteLine("Opción inválida.");

            CleanScreen();
            break;

    }
}

static void CleanScreen()
{
    Console.Write("Ingrese una tecla para continuar...");
    Console.ReadLine();
    Console.Clear();
}