using System;
using System.Collections.Generic;

namespace chat_part2
{
    public class ActivityLogService
    {
        private List<string> activityLog = new List<string>();


        public void AddLog(string action)
        {
            string log = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - {action}";
            activityLog.Add(log);
        }

    
        public List<string> GetLogs()
        {
            return new List<string>(activityLog);
        }

  
        public void ClearLogs()
        {
            activityLog.Clear();
        }

  
        public int LogCount()
        {
            return activityLog.Count;
        }
    }
}