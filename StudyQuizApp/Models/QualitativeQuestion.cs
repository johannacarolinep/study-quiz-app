using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuizApp.Models
{
    public class QualitativeQuestion : Question
    {
        // Fields
        private string answer;

        // Constructor
        public QualitativeQuestion(string questionText, string answer) : base(questionText)
        {
            Answer = answer;
        }

        public QualitativeQuestion(QualitativeQuestion other) : base(other)
        {
            Answer = other.Answer;
        }

        // Properties
        public string Answer 
        {
            get => answer;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Answer cannot be empty or whitespace.");
                }

                answer = value;
            }
        }

        public override QuestionType Type => QuestionType.Qualitative;

        // Methods
        public override string GetCorrectAnswer() => answer;

        public override Question Clone()
        {
            return new QualitativeQuestion(this);
        }
    }
}
