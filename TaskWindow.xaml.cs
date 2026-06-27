using System.Windows;

namespace chat_part2
{
    public partial class TaskWindow : Window
    {
        private TaskService taskService;

        public TaskWindow(TaskService service)
        {
            InitializeComponent();
            taskService = service;
            LoadTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskInput.Text))
                return;

            taskService.AddTask(TaskInput.Text);
            TaskInput.Clear();

            LoadTasks();
        }

        private void BackToChat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void RefreshTasks()
        {
            TaskList.Items.Clear();

            int i = 1;

            foreach (var t in taskService.GetTasks())
            {
                TaskList.Items.Add(
                    $"{i}. {t.Description} - {(t.Completed ? "✔ Done" : "❌ Pending")}");
                i++;
            }
        }
        private void LoadTasks()
        {
            TaskList.Items.Clear();

            int i = 1;

            foreach (var t in taskService.GetTasks())
            {
                TaskList.Items.Add(
                    $"{i}. {t.Description} - {(t.Completed ? "✔ Done" : "❌ Pending")}"
                );
                i++;
            }
        }
    }
}