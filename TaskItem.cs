namespace chat_part2
{
    public class TaskItem
    {
        public string Description { get; set; }

        public bool Completed { get; set; }

        public string Status
        {
            get
            {
                return Completed ? "Completed" : "Pending";
            }
        }
    }
}