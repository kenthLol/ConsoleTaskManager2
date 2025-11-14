using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;
using GestorTareas2.Models;
using GestorTareas2.Interfaces;
using GestorTareas2.Utils;

namespace GestorTareas2.Repositories
{
    public class JsonTaskRepository : IWorkTaskRepository
    {
        private readonly string _filePath;
        private List<WorkTask> _tasks = new List<WorkTask>();

        public JsonTaskRepository(string filePath)
        {
            _filePath = filePath;
            _tasks = JsonHelper.LoadWorkTask<WorkTask>(_filePath);
        }

        public void Add(WorkTask workTask)
        {
            _tasks.Add(workTask);
            JsonHelper.SaveWorkTask(_filePath, _tasks);
        }

        public void Update(WorkTask workTask)
        {
            JsonHelper.SaveWorkTask(_filePath, _tasks);
        }

        public void Delete(int id)
        {
            // Más profesional que buscar y después Remove
            _tasks.RemoveAll(t => t.Id == id);
            JsonHelper.SaveWorkTask(_filePath, _tasks);
        }

        public List<WorkTask> GetAll()
        {
            return _tasks.ToList();
        }

        public WorkTask? GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
    }
}
