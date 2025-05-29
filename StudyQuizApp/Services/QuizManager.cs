using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyQuizApp.Models;

namespace StudyQuizApp.Services
{
    public class QuizManager
    {
        // Fields
        private List<Question> questionList = new List<Question>();
        private List<Attempt> attempts = new List<Attempt>();
        private List<int> incorrectIndices = new List<int>();
        private List<int> activeQuizIndices = new List<int>();
        private int currentIndex = 0;
        private Attempt? currentAttempt;

        public QuizManager() { 
        }

        public int CurrentIndex { get { return currentIndex; } }

        public Attempt CurrentAttempt { get => currentAttempt; }

        // Methods
        public void AddQuestion(Question question) { 
        questionList.Add(question);
        }

        public void UpdateQuestion(Question updatedQuestion, int index)
        {
            if (!CheckIndex(index)) {
                throw new Exception("Could not find the question to update.");
            }
            questionList[index] = updatedQuestion;
        }

        public void DeleteQuestion(int index)
        {
            if (!CheckIndex(index))
            {
                throw new Exception("Failed to delete question. Index not found.");
            }
            questionList.RemoveAt(index);
        }

        public string[] GetQuestionStrings()
        {
            string[] strings = new string[questionList.Count];

            for (int i = 0; i < questionList.Count; i++)
            {
                strings[i] = "Q: " + questionList[i].QuestionText;
            }

            return strings;
        }

        public Question RetrieveQuestion(int index)
        {
            if (!CheckIndex(index)) {
                return null;
            }
            return questionList[index].Clone();
        }

        private bool CheckIndex(int index)
        {
            if (index >= 0 && index < questionList.Count) { return true; }
            return false;
        }

        public void InitiateQuiz()
        {
            currentAttempt = new Attempt(questionList.Count);
            attempts = new List<Attempt>(); // reset attempts
            incorrectIndices = new List<int>(); // reset incorrectIndices
            activeQuizIndices = Enumerable.Range(0, questionList.Count).ToList();
            currentIndex = 0;
        }

        public void StartRetry()
        {
            currentIndex = 0;
            activeQuizIndices = incorrectIndices.ToList();
            currentAttempt = new Attempt(activeQuizIndices.Count);
            incorrectIndices = new List<int>();
        }

        public Question GetCurrentQuestion()
        {
            if (currentIndex >= 0 && currentIndex < activeQuizIndices.Count)
            {
                int realIndex = activeQuizIndices[currentIndex];
                return RetrieveQuestion(realIndex);
            }
            return null;
        }

        public void MarkCorrect() => currentAttempt.AddPoint();

        public void MarkIncorrect()
        {
            incorrectIndices.Add(activeQuizIndices[currentIndex]);
        }

        public void IncrementCurrentIndex()
        {
            if (currentIndex < activeQuizIndices.Count) currentIndex++;
        }

        public void FinishCurrentAttempt()
        {
            if (currentAttempt != null)
            {
                attempts.Add(currentAttempt);
                currentAttempt = null;
            }
        }

        public bool IsQuizComplete() => currentIndex >= activeQuizIndices.Count;

        public bool IsLastQuestion() => currentIndex == activeQuizIndices.Count - 1;

        public int CountIncorrectIndices() => incorrectIndices.Count;

        public List<string> GetResultsSummaries()
        {
            List<string> results = new List<string>();
            for (int i = 0; i < attempts.Count; i++)
            {
                string attemptResult = i == 0 ? "Primary quiz run:\n" : $"Re-run {i}\n";
                attemptResult += $"{attempts[i].ToString()}";
                results.Add(attemptResult);
            }

            return results;
        }
    }
}
