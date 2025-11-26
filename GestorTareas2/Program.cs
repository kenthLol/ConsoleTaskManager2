using GestorTareas2.Interfaces;
using GestorTareas2.Models;
using GestorTareas2.Repositories;
using GestorTareas2.Services;
using System.Reflection;
using System.Threading.Tasks;

string filePath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
IWorkTaskRepository workTaskRepository = new JsonTaskRepository(filePath);

TaskManager taskManager = new TaskManager(workTaskRepository);

//Definir los anchos de las columnas para el formato de tabla
const int IdWidth = 4;
const int TitleWidth = 25;
const int StateWidth = 12;
const int PriorityWidth = 10;
const int DescriptionWidth = 40;

while (true)
{
    Console.WriteLine("\n--- GESTOR DE TAREAS ---");
    Console.WriteLine("1. Agregar tarea");
    Console.WriteLine("2. Listar tareas");
    Console.WriteLine("3. Completar tarea");
    Console.WriteLine("4. Eliminar tarea");
    Console.WriteLine("5. Buscar tarea");
    Console.WriteLine("6. Filtrar tareas por prioridad");
    Console.WriteLine("7. Obtener tareas ordenadas");
    Console.WriteLine("8. Hay tarea urgente?");
    Console.WriteLine("9. Cuantas tareas hay por prioridad?");
    Console.WriteLine("10. Salir");
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
                Console.WriteLine("\n--- LISTA DE TAREAS PENDIENTES Y COMPLETADAS ---");

                // Definir la línea de separación
                string separator = new string('-', IdWidth + TitleWidth + StateWidth + PriorityWidth + DescriptionWidth + 8); // +8 para los espacios entre columnas
                Console.WriteLine(separator);

                // Formato del encabezado usando alineación a la izquierda (-) y los anchos definidos
                Console.WriteLine(
                    $"{"ID",-IdWidth} | " +
                    $"{"TÍTULO",-TitleWidth} | " +
                    $"{"ESTADO",-StateWidth} | " +
                    $"{"PRIORIDAD",-PriorityWidth} | " +
                    $"{"DESCRIPCIÓN",-DescriptionWidth}"
                );

                Console.WriteLine(separator);

                foreach (WorkTask task in results)
                {
                    string state = task.IsCompleted ? "Completada" : "Pendiente";
                    string priorityName = task.Priority.ToString();

                    // Si la descripción es demasiado larga, truncarla para que encaje en la columna
                    string taskDescription = task.Description;
                    if (taskDescription.Length > DescriptionWidth)
                    {
                        taskDescription = taskDescription.Substring(0, DescriptionWidth - 3) + "...";
                    }

                    // Formato de la línea de datos usando los anchos definidos
                    Console.WriteLine(
                        $"{task.Id,-IdWidth} | " +
                        $"{task.Title,-TitleWidth} | " +
                        $"{state,-StateWidth} | " +
                        $"{priorityName,-PriorityWidth} | " +
                        $"{taskDescription,-DescriptionWidth}"
                    );
                }

                Console.WriteLine(separator);
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
            List<WorkTask> workTaskOrdered = taskManager.GetWorkTaskOrdered();

            if(workTaskOrdered.Count == 0)
            {
                Console.WriteLine("No hay tareas");
            }
            else
            {
                Console.WriteLine("Las tareas se muestran según su prioridad en orden descendente");

                foreach (var workTask in workTaskOrdered)
                {
                    string state = workTask.IsCompleted ? "Completada" : "Pendiente";
                    Console.WriteLine($"{workTask.Id} - {workTask.Title} - {workTask.Priority}: {state}");
                }
            }
            CleanScreen();
            break;
        case "8":
            bool urgentWorkTask = taskManager.HasUrgentWorkTask();

            if (!urgentWorkTask)
            {
                Console.WriteLine("No hay tareas urgentes");
            }
            else
            {
                Console.WriteLine("Hay tareas urgentes");
            }
            CleanScreen();
            break;
        case "9":
            Dictionary<Priority, int> result = taskManager.GetTasksCountByPriority();

            if(result.Count > 0)
            {
                Console.WriteLine("Cantidad de tareas por prioridad: ");
                foreach (var workTask in result)
                {
                    Console.WriteLine($"{workTask.Key} => {workTask.Value}");
                }
            }
            else
            {
                Console.WriteLine("No hay tareas");
            }

            CleanScreen();
            break;
        case "10":
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