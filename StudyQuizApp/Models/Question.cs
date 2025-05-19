using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuizApp.Models
{
    public abstract class Question
    {
        // Fields
        private string questionText;

        // Constructor
        public Question(string questionText)
        {
            QuestionText = questionText;
        }

        // Properties
        public string QuestionText 
        {
            get => questionText;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Question text cannot be empty or whitespace.");
                }
                if (value.Length > 200)
                {
                    throw new ArgumentException("Question text cannot exceed 200 characters.");
                }

                questionText = value;
            }
        }

        public abstract QuestionType Type { get; }

        // Methods
        public abstract string GetCorrectAnswer();
    }
}
