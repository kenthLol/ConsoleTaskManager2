using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GestorTareas2.Interfaces;
using GestorTareas2.Models;
using GestorTareas2.Utils;

namespace GestorTareas2.Services;

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

    public List<WorkTask> GetAllWorkTaskList()
    {
        var workTask = _workTaskRepository.GetAll();

        return workTask;
    }

    public bool CompleteWorkTask(int id)
    {
        var workTask = _workTaskRepository.GetById(id);

        if (workTask == null)
        {
            return false;
        }

        workTask.IsCompleted = true;
        _workTaskRepository.Update(workTask);

        return true;
    }

    public bool DeleteWorkTask(int id)
    {
        var workTask = _workTaskRepository.GetById(id);

        if (workTask == null)
        {
            return false;
        }

        _workTaskRepository.Delete(id);

        return true;
    }

    public List<WorkTask> SearchWorkTask(string text)
    {
        var foundWorkTask = _workTaskRepository.GetAll()
            .Where(wt =>
                (wt.Title?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (wt.Description?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false)
            )
            .ToList();

        return foundWorkTask;
    }

    public List<WorkTask> FilterWorkTasksByPriority(Priority priority)
    {
        var filteredTasks = _workTaskRepository.GetAll()
            .Where(t => t.Priority == priority)
            .ToList();

        return filteredTasks;
    }

    public List<WorkTask> GetWorkTaskOrdered()
    {
        var workTask = _workTaskRepository.GetAll();

        var workTaskOrdered = workTask
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => !t.IsCompleted)
            .ThenBy(t => t.Title)
            .ToList();

        return workTaskOrdered;
    }

    public List<string> GetWorkTaskTitles()
    {
        var workTaskTitle = _workTaskRepository.GetAll()
            .Select(t => t.Title)
            .ToList();

        return workTaskTitle;
    }

    public bool HasUrgentWorkTask()
    {
        bool urgentWorkTask = _workTaskRepository.GetAll()
            .Any(t => t.Priority == Priority.Alta && !t.IsCompleted);

        return urgentWorkTask;
    }

    public List<WorkTask> GetLatestWorkTask()
    {
        var latestWorkTask = _workTaskRepository.GetAll()
            .OrderByDescending(t => t.CreationDate)
            .Take(3)
            .ToList();

        return latestWorkTask;
    }

    public List<object> GetWorkTaskSummary()
    {
        var summary = _workTaskRepository.GetAll()
            .Select(t => new {
                t.Id,
                t.Title,
                State = t.IsCompleted ? "Completado" : "Incompleto"
            })
            .Cast<object>()
            .ToList();

        return summary;
    }

    public Dictionary<Priority, int> GetTasksCountByPriority()
    {
        Dictionary<Priority, int> workTasks = _workTaskRepository.GetAll()
            .GroupBy(t => t.Priority)
            .ToDictionary(t => t.Key, t => t.Count());

        return workTasks;
    }

    public Dictionary<string, int> GetTasksCountByState()
    {
        var workTasks = _workTaskRepository.GetAll()
            .GroupBy(t => t.IsCompleted)
            .ToDictionary(t => t.Key, t => t.Count());

        Dictionary<string, int> countsByState = new()
        {
            { "Completadas", workTasks.GetValueOrDefault(true, 0) },
            { "Pendientes", workTasks.GetValueOrDefault(false, 0) }
        };

        return countsByState;
    }

    public Dictionary<DateTime, List<WorkTask>> GetTasksGroupedByDate()
    {
        var workTasks = _workTaskRepository.GetAll()
            .GroupBy(t => t.CreationDate.Date);

        return workTasks
            .ToDictionary(t => t.Key, t => t.ToList());
    }

    public object GetDashboardStats()
    {
        List<WorkTask> workTasks = _workTaskRepository.GetAll();

        int totalWorkTasks = workTasks.Count();

        Dictionary<bool, int> PendingAndCompleted = workTasks
            .GroupBy(t => t.IsCompleted)
            .ToDictionary(t => t.Key, t => t.Count());

        Dictionary<string, int> totalPendingAndCompleted = new()
        {
            { "Completadas", PendingAndCompleted.GetValueOrDefault(true, 0)},
            { "Pendientes", PendingAndCompleted.GetValueOrDefault(false, 0)}
        };

        double percentageCompleted = 0;
        if (totalWorkTasks != 0)
        {
            double result = (double)totalPendingAndCompleted["Completadas"] / totalWorkTasks * 100;
            percentageCompleted = Math.Round(result, 2);
        }
        else
        {
            percentageCompleted = 0;
        }

        return new
        {
            Total = totalWorkTasks,
            Completadas = totalPendingAndCompleted["Completadas"],
            Pendientes = totalPendingAndCompleted["Pendientes"],
            PorcentajeCompletadas = percentageCompleted
        };
    }
}