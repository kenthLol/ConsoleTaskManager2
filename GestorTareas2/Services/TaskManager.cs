using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GestorTareas2.Interfaces;
using GestorTareas2.Models;
using GestorTareas2.Utils;
using GestorTareas2.Extensions;

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
            .Search(text)
            .ToList();

        return foundWorkTask;
    }

    public List<WorkTask> FilterWorkTasksByPriority(Priority priority)
    {
        var filteredTasks = _workTaskRepository.GetAll()
            .ByPriority(priority)
            .ToList();

        return filteredTasks;
    }

    public List<WorkTask> GetTaskOrdered()
    {
        return _workTaskRepository
            .GetAll()
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.Title)
            .Pending()
            .ToList();
    }

    public List<string> GetTaskTitles()
    {
        return _workTaskRepository
            .GetAll()
            .Select(t => t.Title)
            .Where(title => title != null)
            .ToList();
    }

    public bool HasUrgentTasks()
    {
        return _workTaskRepository
            .GetAll()
            .Any(t =>
                t.Priority == Priority.Alta &&
                !t.IsCompleted
            );
    }

    public List<WorkTask> GetLatestTask()
    {
        return _workTaskRepository.GetAll()
            .OrderByDescending(t => t.CreationDate)
            .Latest(3)
            .ToList();
    }

    public List<object> GetTaskSummary()
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

        return _workTaskRepository.GetAll()
            .GroupBy(t => t.CreationDate.Date)
            .OrderBy(t => t.Key)
            .ToDictionary(t => t.Key, t => t.ToList());
    }

    public object GetDashboardStats()
    {
        List<WorkTask> allTasks = _workTaskRepository.GetAll();

        var completionStats = allTasks
            .GroupBy(task => task.IsCompleted)
            .ToDictionary(
                group => group.Key, 
                group => group.Count()
            );

        int totalCompleted = completionStats.GetValueOrDefault(true, 0);
        int totalPending = completionStats.GetValueOrDefault(false, 0);
        int totalTasks = totalCompleted + totalPending;

        double completionPercentage = (totalTasks != 0)
            ? Math.Round(((double)totalCompleted / totalTasks) * 100, 2) : 0.0;

        return new
        {
            TotalTareas = totalTasks,
            Completadas = totalCompleted,
            Pendientes = totalPending,
            PorcentajeCompletadas = completionPercentage
        };
    }
}