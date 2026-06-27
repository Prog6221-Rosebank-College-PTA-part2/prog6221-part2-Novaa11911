using Professor_Bot_GUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;

namespace chat_part2
{
    public partial class MainWindow : Window
    {
        ResponsesService response = new ResponsesService();
        DatabaseService db = new DatabaseService();
        ActivityLogService activity = new ActivityLogService();
        TaskService taskService = new TaskService();
        private string userName = "";
        QuizService quiz = new QuizService();
        bool quizRunning = false;
        Dictionary<string, string> memory = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();

            Paragraph startParagraph = new Paragraph();

            Run botName = new Run("#Nova-Bot: ");
            botName.Foreground = Brushes.LimeGreen;
            botName.FontWeight = FontWeights.Bold;

            Run botText = new Run("Please enter your name first.");
            botText.Foreground = Brushes.White;

            startParagraph.Inlines.Add(botName);
            startParagraph.Inlines.Add(botText);

            txtChat.Document.Blocks.Add(startParagraph);

            response.SpeakMessage("Please enter your name first.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = txtMessage.Text.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            string lowerMessage = userMessage.ToLower();




            if (lowerMessage.StartsWith("add a task") || lowerMessage.StartsWith("add task"))
            {
                string taskText = userMessage;

                taskText = taskText
                    .Replace("add a task", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("add task", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();

                if (!string.IsNullOrWhiteSpace(taskText))
                {
                    taskService.AddTask(taskText);

                   
                    db.SaveTask(userName, taskText);

                    activity.AddLog("Task added via chat: " + taskText);

                    Paragraph p = new Paragraph();
                    p.Inlines.Add(new Run("#Nova-Bot: Task saved = " + taskText));

                    txtChat.Document.Blocks.Add(p);
                }
                else
                {
                    txtChat.Document.Blocks.Add(
                        new Paragraph(new Run("#Nova-Bot: Please specify a task"))
                    );
                }

                txtMessage.Clear();
                return;
            }


            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = userMessage;
                memory["name"] = userName;

                activity.AddLog("User entered name: " + userName);

    
                Paragraph userParagraph = new Paragraph();

                Run userLabel = new Run(userName + ": ");
                userLabel.Foreground = Brushes.Red;
                userLabel.FontWeight = FontWeights.Bold;

                Run userText = new Run(userName);
                userText.Foreground = Brushes.White;

                userParagraph.Inlines.Add(userLabel);
                userParagraph.Inlines.Add(userText);

                txtChat.Document.Blocks.Add(userParagraph);

                string welcomeMessage =
                    "Hello " + userName +
                    ". Welcome to Nova-Bot, a cybersecurity awareness bot. " +
                    "I'm here to help ,ask me anything. " ;

                Paragraph welcomeParagraph = new Paragraph();

                Run botLabel = new Run("#Nova-Bot: ");
                botLabel.Foreground = Brushes.LimeGreen;
                botLabel.FontWeight = FontWeights.Bold;

                Run welcomeText = new Run(welcomeMessage);
                welcomeText.Foreground = Brushes.White;

                welcomeParagraph.Inlines.Add(botLabel);
                welcomeParagraph.Inlines.Add(welcomeText);

                txtChat.Document.Blocks.Add(welcomeParagraph);

                response.SpeakMessage(welcomeMessage);

                txtMessage.Clear();
                return;
            }


            if (lowerMessage == "show chat history")
            {
                activity.AddLog("Viewed chat history");

                List<string> history = db.GetChatHistory(userName);

                if (history.Count == 0)
                {
                    Paragraph emptyParagraph = new Paragraph();

                    emptyParagraph.Inlines.Add(
                        new Run("#Nova-Bot: ")
                        {
                            Foreground = Brushes.LimeGreen,
                            FontWeight = FontWeights.Bold
                        });

                    emptyParagraph.Inlines.Add(
                        new Run("No previous chats were found.")
                        {
                            Foreground = Brushes.White
                        });

                    txtChat.Document.Blocks.Add(emptyParagraph);
                }
                else
                {
                    foreach (string line in history)
                    {
                        Paragraph historyParagraph = new Paragraph();

                        historyParagraph.Inlines.Add(
                            new Run(line)
                            {
                                Foreground = Brushes.White
                            });

                        txtChat.Document.Blocks.Add(historyParagraph);
                    }
                }

                txtMessage.Clear();
                return;
            }


            if (lowerMessage == "show activity")
            {
                Paragraph heading = new Paragraph();

                heading.Inlines.Add(new Run("#Nova-Bot: ")
                {
                    Foreground = Brushes.LimeGreen,
                    FontWeight = FontWeights.Bold
                });

                heading.Inlines.Add(new Run("Activity Log")
                {
                    Foreground = Brushes.White
                });

                txtChat.Document.Blocks.Add(heading);

                foreach (string log in activity.GetLogs())
                {
                    Paragraph p = new Paragraph();

                    p.Inlines.Add(new Run(log)
                    {
                        Foreground = Brushes.White
                    });

                    txtChat.Document.Blocks.Add(p);
                }

                txtMessage.Clear();
                return;
            }

            if (lowerMessage == "clear activity")
            {
                activity.ClearLogs();

                Paragraph p = new Paragraph();

                p.Inlines.Add(new Run("#Nova-Bot: ")
                {
                    Foreground = Brushes.LimeGreen,
                    FontWeight = FontWeights.Bold
                });

                p.Inlines.Add(new Run("Activity log cleared.")
                {
                    Foreground = Brushes.White
                });

                txtChat.Document.Blocks.Add(p);

                txtMessage.Clear();
                return;
            }


            if (lowerMessage.StartsWith("add task") ||
                lowerMessage.StartsWith("remind me") ||
                lowerMessage.StartsWith("remember to"))
            {
                string task = userMessage;

                task = task.Replace("Add task", "")
                           .Replace("add task", "")
                           .Replace("Remind me", "")
                           .Replace("remind me", "")
                           .Replace("Remember to", "")
                           .Replace("remember to", "")
                           .Trim();

                taskService.AddTask(task);

                activity.AddLog("Task added: " + task);

                Paragraph p = new Paragraph();

                p.Inlines.Add(new Run("#Nova-Bot: ")
                {
                    Foreground = Brushes.LimeGreen,
                    FontWeight = FontWeights.Bold
                });

                p.Inlines.Add(new Run("Task added successfully!")
                {
                    Foreground = Brushes.White
                });

                txtChat.Document.Blocks.Add(p);

                response.SpeakMessage("Task added successfully.");

                txtMessage.Clear();
                return;
            }

            if (lowerMessage == "show tasks")
            {
                Paragraph heading = new Paragraph();

                heading.Inlines.Add(new Run("#Nova-Bot: ")
                {
                    Foreground = Brushes.LimeGreen,
                    FontWeight = FontWeights.Bold
                });

                heading.Inlines.Add(new Run("Your Tasks")
                {
                    Foreground = Brushes.White
                });

                txtChat.Document.Blocks.Add(heading);

                var tasks = taskService.GetTasks();

                if (tasks.Count == 0)
                {
                    Paragraph p = new Paragraph();

                    p.Inlines.Add(new Run("No tasks available.")
                    {
                        Foreground = Brushes.White
                    });

                    txtChat.Document.Blocks.Add(p);
                }
                else
                {
                    int i = 1;

                    foreach (var task in tasks)
                    {
                        Paragraph p = new Paragraph();

                        string status = task.Completed ? "✔ Completed" : "❌ Pending";

                        p.Inlines.Add(new Run($"{i}. {task.Description} - {status}")
                        {
                            Foreground = Brushes.White
                        });

                        txtChat.Document.Blocks.Add(p);

                        i++;
                    }
                }

                activity.AddLog("Viewed tasks");

                txtMessage.Clear();
                return;
            }


            if (lowerMessage.StartsWith("complete task"))
            {
                string number =
                    lowerMessage.Replace("complete task", "").Trim();

                if (int.TryParse(number, out int taskNo))
                {
                    if (taskService.CompleteTask(taskNo))
                    {
                        activity.AddLog("Completed task " + taskNo);

                        MessageBox.Show("Task completed.");
                    }
                    else
                    {
                        MessageBox.Show("Task not found.");
                    }
                }

                txtMessage.Clear();
                return;
            }


            if (lowerMessage.StartsWith("delete task"))
            {
                string number =
                    lowerMessage.Replace("delete task", "").Trim();

                if (int.TryParse(number, out int taskNo))
                {
                    if (taskService.DeleteTask(taskNo))
                    {
                        activity.AddLog("Deleted task " + taskNo);

                        MessageBox.Show("Task deleted.");
                    }
                    else
                    {
                        MessageBox.Show("Task not found.");
                    }
                }

                txtMessage.Clear();
                return;
            }


        
            if (quizRunning)
            {
                int answer = -1;

                switch (lowerMessage)
                {
                    case "a":
                        answer = 0;
                        break;

                    case "b":
                        answer = 1;
                        break;

                    case "c":
                        answer = 2;
                        break;

                    case "d":
                        answer = 3;
                        break;
                }

                if (answer == -1)
                {
                    MessageBox.Show("Please answer using A, B, C or D.");
                    txtMessage.Clear();
                    return;
                }

                bool finished = quiz.SubmitAnswer(answer);

                if (finished)
                {
                    quizRunning = false;

                    string result =
                        $"Quiz Complete!\n\nYour Score: {quiz.Score}/{quiz.TotalQuestions}";

                    Paragraph p = new Paragraph();

                    p.Inlines.Add(new Run("#Nova-Bot: ")
                    {
                        Foreground = Brushes.LimeGreen,
                        FontWeight = FontWeights.Bold
                    });

                    p.Inlines.Add(new Run(result));

                    txtChat.Document.Blocks.Add(p);

                    response.SpeakMessage(result);

                    activity.AddLog("Quiz completed");
                }
                else
                {
                    var q = quiz.GetCurrentQuestion();

                    Paragraph p = new Paragraph();

                    p.Inlines.Add(new Run("#Nova-Bot: ")
                    {
                        Foreground = Brushes.LimeGreen,
                        FontWeight = FontWeights.Bold
                    });

                    p.Inlines.Add(new Run(q.Question));

                    txtChat.Document.Blocks.Add(p);

                    foreach (string option in q.Options)
                    {
                        txtChat.Document.Blocks.Add(
                            new Paragraph(new Run(option)));
                    }
                }

                txtMessage.Clear();
                return;
            }
            if (lowerMessage == "start quiz")
            {
                quiz.ResetQuiz();

                quizRunning = true;

                activity.AddLog("Quiz started");

                var q = quiz.GetCurrentQuestion();

                Paragraph p = new Paragraph();

                p.Inlines.Add(new Run("#Nova-Bot: ")
                {
                    Foreground = Brushes.LimeGreen,
                    FontWeight = FontWeights.Bold
                });

                p.Inlines.Add(new Run(q.Question));

                txtChat.Document.Blocks.Add(p);

                foreach (string option in q.Options)
                {
                    txtChat.Document.Blocks.Add(
                        new Paragraph(new Run(option)));
                }

                txtMessage.Clear();

                return;
            }



           
            Paragraph normalUserParagraph = new Paragraph();

            Run normalUserLabel = new Run(userName + ": ");
            normalUserLabel.Foreground = Brushes.Red;
            normalUserLabel.FontWeight = FontWeights.Bold;

            Run normalUserText = new Run(userMessage);
            normalUserText.Foreground = Brushes.White;

            normalUserParagraph.Inlines.Add(normalUserLabel);
            normalUserParagraph.Inlines.Add(normalUserText);

            txtChat.Document.Blocks.Add(normalUserParagraph);

            activity.AddLog("User asked: " + userMessage);


         
            string botReply = response.GetResponse(userMessage, userName);

            Paragraph botParagraph = new Paragraph();

            Run botReplyLabel = new Run("#Nova-Bot: ");
            botReplyLabel.Foreground = Brushes.LimeGreen;
            botReplyLabel.FontWeight = FontWeights.Bold;

            Run botReplyText = new Run(botReply);
            botReplyText.Foreground = Brushes.White;

            botParagraph.Inlines.Add(botReplyLabel);
            botParagraph.Inlines.Add(botReplyText);

            txtChat.Document.Blocks.Add(botParagraph);

            response.SpeakMessage(botReply);

           
            db.SaveChat(userName, userMessage, botReply);

            activity.AddLog("Conversation saved to database");

            txtMessage.Clear();
        }

        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow win = new TaskWindow(taskService);
            win.Show();
        }

        private void OpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            QuizWindow win = new QuizWindow(quiz);
            win.Show();
        }
    }
}