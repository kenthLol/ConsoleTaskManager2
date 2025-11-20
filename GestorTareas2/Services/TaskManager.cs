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

    public List<WorkTask> WorkTaskList()
    {
        var workTask = _workTaskRepository.GetAll();

        if (workTask == null)
        {
            return new List<WorkTask>();
        }

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
}