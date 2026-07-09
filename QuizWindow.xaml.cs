using System.Windows;
using System.Windows.Controls;

namespace chat_part2
{
    public partial class QuizWindow : Window
    {
        private QuizService quiz;

        public QuizWindow(QuizService service)
        {
            InitializeComponent();
            quiz = service;

            quiz.ResetQuiz();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            var q = quiz.GetCurrentQuestion();

            if (q == null)
            {
                QuestionText.Text = "Quiz Completed!";
                ScoreText.Text = $"Score: {quiz.Score}/{quiz.TotalQuestions}";
                return;
            }

            QuestionText.Text = q.Question;

            BtnA.Content = q.Options[0];
            BtnB.Content = q.Options[1];

            BtnC.Visibility = q.Options.Length > 2 ? Visibility.Visible : Visibility.Collapsed;
            BtnD.Visibility = q.Options.Length > 3 ? Visibility.Visible : Visibility.Collapsed;

            if (q.Options.Length > 2) BtnC.Content = q.Options[2];
            if (q.Options.Length > 3) BtnD.Content = q.Options[3];

            ScoreText.Text = $"Score: {quiz.Score}";
        }
        private void BackToChat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            int answer = -1;

            if (btn == BtnA) answer = 0;
            if (btn == BtnB) answer = 1;
            if (btn == BtnC) answer = 2;
            if (btn == BtnD) answer = 3;

            quiz.SubmitAnswer(answer);

            LoadQuestion();
        }
    }
}