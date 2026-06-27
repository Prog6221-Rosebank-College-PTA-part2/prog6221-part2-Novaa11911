using System.Collections.Generic;

namespace chat_part2
{
    public class TaskService
    {
        private List<TaskItem> tasks = new List<TaskItem>();


        public void AddTask(string description)
        {
            tasks.Add(new TaskItem
            {
                Description = description,
                Completed = false
            });
        }

        public List<TaskItem> GetTasks()
        {
            return new List<TaskItem>(tasks);
        }

      
        public bool CompleteTask(int number)
        {
            if (number < 1 || number > tasks.Count)
                return false;

            tasks[number - 1].Completed = true;
            return true;
        }

   
        public bool DeleteTask(int number)
        {
            if (number < 1 || number > tasks.Count)
                return false;

            tasks.RemoveAt(number - 1);
            return true;
        }

      
        public void ClearTasks()
        {
            tasks.Clear();
        }


        public int TaskCount()
        {
            return tasks.Count;
        }
    }
}