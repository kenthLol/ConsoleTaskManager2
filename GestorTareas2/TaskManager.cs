using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GestorTareas.Models;
using GestorTareas.Utils;

namespace GestorTareas;

public class TaskManager
{
    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "tasks.json");
    private List<WorkTask> _workTasks = new List<WorkTask>();

    public TaskManager()
    {
        _workTasks = JsonHelper.LoadWorkTask<WorkTask>(FilePath);
    }

    public void AddWorkTask(string title, string description, Priority priority)
    {
        int newId = _workTasks.Count > 0 ? _workTasks.Max(t => t.Id) + 1 : 1;

        var workTask = new WorkTask()
        {
            Id = newId,
            Title = title,
            Description = description,
            Priority = priority
        };

        _workTasks.Add(workTask);

        JsonHelper.SaveWorkTask(FilePath, _workTasks);
    }

    public void WorkTaskList()
    {
        if (_workTasks.Count == 0)
        {
            Console.WriteLine("No hay tareas registradas.");
            return;
        }

        foreach (var workTask in _workTasks)
        {
            string state = workTask.IsCompleted ? "Completada" : "Pendiente";
            Console.WriteLine($"{workTask.Id}. {workTask.Title} - {state}");
        }
    }

    public void CompleteWorkTask(int id)
    {
        var workTask = _workTasks.FirstOrDefault(t => t.Id == id);

        if (workTask == null)
        {
            Console.WriteLine("Tarea no encontrada");

            return;
        }

        workTask.IsCompleted = true;
        JsonHelper.SaveWorkTask(FilePath, _workTasks);
    }

    public void DeleteWorkTask(int id)
    {
        var workTask = _workTasks.FirstOrDefault(t => t.Id == id);

        if (workTask == null)
        {
            Console.WriteLine("Tarea no encontrada");
            return;
        }

        _workTasks.Remove(workTask);
        JsonHelper.SaveWorkTask(FilePath, _workTasks);
    }

    public void SearchWorkTask(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Debe ingresar un texto válido");
            return;
        }

        var foundWorkTask = _workTasks
            .Where(wt =>
                (wt.Title?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (wt.Description?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false)
            )
            .ToList();

        if (foundWorkTask.Count == 0)
        {
            Console.WriteLine("No se encontraron tareas que coincidan con el texto proporcionado.");
            return;
        }

        foreach (var workTask in foundWorkTask)
        {
            string state = workTask.IsCompleted ? "Completada" : "Pendiente";
            Console.WriteLine($"{workTask.Id}. {workTask.Title} - {state}");
        }
    }

    public void FilterWorkTasksByPriority(Priority priority)
    {
        var filteredTasks = _workTasks
            .Where(t => t.Priority == priority)
            .ToList();

        if (filteredTasks.Count == 0)
        {
            Console.WriteLine("No hay tareas con esa prioridad");
            return;
        }

        foreach (var workTask in filteredTasks)
        {
            Console.WriteLine($"{workTask.Id} - {workTask.Title} - {workTask.Priority}");
        }
    }

    public void CleanScreen()
    {
        Console.Write("Ingrese una tecla para continuar...");
        Console.ReadLine();
        Console.Clear();
    }
}