using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuizApp.Models
{
    public class Attempt
    {
        // Field
        private int questionsCount;
        private int correctCount = 0;

        // Constructor
        public Attempt(int totalQuestions) 
        {
            this.questionsCount = totalQuestions;
        }

        // Properties
        public int QuestionsCount { get => questionsCount; }
        public int CorrectCount { get => correctCount; }


        // Methods
        public void AddPoint() => correctCount++;

        public double GetRelativePoint(int? questionsAnswered = null)
        {
            int denominator = questionsAnswered ?? questionsCount;
            return denominator > 0 ? (double)correctCount / denominator : 0;
        }

        public override string ToString()
        {
            return $"Score: {CorrectCount}/{QuestionsCount} ({GetRelativePoint() * 100:F0}%)";
        }
    }
}
