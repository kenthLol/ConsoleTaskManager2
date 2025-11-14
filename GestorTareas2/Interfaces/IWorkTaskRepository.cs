using GestorTareas2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTareas2.Interfaces
{
    public interface IWorkTaskRepository
    {
        void Add(WorkTask workTask);
        void Update(WorkTask workTask);
        void Delete(int id);
        WorkTask? GetById(int id);
        List<WorkTask> GetAll();
    }
}
