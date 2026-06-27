using System.Collections.Generic;

namespace chat_part2
{
    public class QuizService
    {
        private List<QuizQuestion> questions = new List<QuizQuestion>();

        private int currentQuestion = 0;
        private int score = 0;

        public QuizService()
        {
            questions.Add(new QuizQuestion
            {
                Question = "What is the safest password?",
                Options = new string[]
                {
                    "A. password123",
                    "B. 12345678",
                    "C. T7#xP!92Lm",
                    "D. qwerty"
                },
                CorrectAnswer = 2
            });

            questions.Add(new QuizQuestion
            {
                Question = "What does phishing try to steal?",
                Options = new string[]
                {
                    "A. Your login details",
                    "B. Your keyboard",
                    "C. Your monitor",
                    "D. Your printer"
                },
                CorrectAnswer = 0
            });

            questions.Add(new QuizQuestion
            {
                Question = "Should you use public Wi-Fi without protection?",
                Options = new string[]
                {
                    "A. Yes",
                    "B. No"
                },
                CorrectAnswer = 1
            });

            questions.Add(new QuizQuestion
            {
                Question = "What does VPN stand for?",
                Options = new string[]
                {
                    "A. Virtual Private Network",
                    "B. Very Personal Network",
                    "C. Virtual Password Number",
                    "D. Verified Private Node"
                },
                CorrectAnswer = 0
            });

            questions.Add(new QuizQuestion
            {
                Question = "What should you do with suspicious email links?",
                Options = new string[]
                {
                    "A. Click them",
                    "B. Ignore/Delete them",
                    "C. Share them",
                    "D. Reply immediately"
                },
                CorrectAnswer = 1
            });
        }

    
        public QuizQuestion GetCurrentQuestion()
        {
            if (currentQuestion < 0 || currentQuestion >= questions.Count)
                return null;

            return questions[currentQuestion];
        }

        public bool SubmitAnswer(int answer)
        {
            if (currentQuestion >= questions.Count)
                return true;

            if (questions[currentQuestion].CorrectAnswer == answer)
                score++;

            currentQuestion++;

            return currentQuestion >= questions.Count;
        }

        public bool IsFinished()
        {
            return currentQuestion >= questions.Count;
        }

        public int Score => score;
        public int TotalQuestions => questions.Count;

        public void ResetQuiz()
        {
            currentQuestion = 0;
            score = 0;
        }
    }
}