using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GestorTareas2.Interfaces;
using GestorTareas2.Models;
using GestorTareas2.Utils;

namespace GestorTareas;

public class TaskManager
{
    private readonly IWorkTaskRepository _workTaskRepository;
    public TaskManager(IWorkTaskRepository workTaskRepository)
    {
        _workTaskRepository = workTaskRepository;
    }

    public void AddWorkTask(string title, string description, Priority priority)
    {
        var workTask = new WorkTask()
        {
            Id = GenerateId(),
            Title = title,
            Description = description,
            Priority = priority
        };

        _workTaskRepository.Add(workTask);
    }

    public int GenerateId()
    {
        var workTask = _workTaskRepository.GetAll();
        return workTask.Count == 0 ? 1 : workTask.Max(t => t.Id) + 1;
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