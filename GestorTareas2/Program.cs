using GestorTareas;
using GestorTareas.Models;

TaskManager taskManager = new TaskManager();

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
            taskManager.WorkTaskList();
            taskManager.CleanScreen();
            break;
        case "3":
            Console.WriteLine("Id de la tarea a completar: ");

            if (int.TryParse(Console.ReadLine(), out int idCompletar))
            {
                taskManager.CompleteWorkTask(idCompletar);
            }

            taskManager.CleanScreen();
            break;
        case "4":
            Console.WriteLine("Id de la tarea a eliminar: ");

            if (int.TryParse(Console.ReadLine(), out int idEliminar))
            {
                taskManager.DeleteWorkTask(idEliminar);
            }

            taskManager.CleanScreen();
            break;
        case "5":
            Console.WriteLine("Buscar (Titulo o Descripcion): ");
            string text = Console.ReadLine() ?? string.Empty;
            taskManager.SearchWorkTask(text);

            taskManager.CleanScreen();
            break;

        case "6":
            Console.WriteLine("Buscar por Prioridad (1.Baja, 2.Media, 3.Alta): ");
            string? priorityStrSort = Console.ReadLine();

            if (!int.TryParse(priorityStrSort, out int priority) || !Enum.IsDefined(typeof(Priority), priority))
            {
                priority = (int)Priority.Media;
            }

            taskManager.FilterWorkTasksByPriority((Priority)priority);

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