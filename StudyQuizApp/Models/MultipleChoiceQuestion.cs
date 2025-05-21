using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuizApp.Models
{
    public class MultipleChoiceQuestion : Question
    {
        // Fields
        private string[] options;
        private int correctIndex;

        // Constructor
        public MultipleChoiceQuestion(string questionText, string[] options, int correctIndex) : base(questionText)
        {
            Options = options;
            CorrectIndex = correctIndex;
        }

        public MultipleChoiceQuestion(MultipleChoiceQuestion other) : base(other)
        {
            Options = other.Options;
            CorrectIndex = other.CorrectIndex;
        }

        // Properties
        public override QuestionType Type => QuestionType.MultipleChoice;

        public string[] Options 
        {
            get { return (string[])options.Clone(); }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Options), "Options array cannot be null.");

                if (value.Length != 4)
                    throw new ArgumentException("Options array must contain exactly 4 items.");

                if (value.Any(opt => string.IsNullOrWhiteSpace(opt)))
                    throw new ArgumentException("Each option must be a non-empty, non-whitespace string.");

                options = value;
            }
        }

        public int CorrectIndex 
        { 
            get { return correctIndex; }
            set
            {
                if (value < 0 || value > 3)
                    throw new ArgumentOutOfRangeException(nameof(CorrectIndex), "You must mark one of the options as correct.");

                correctIndex = value;
            }
        }

        // Methods
        public override string GetCorrectAnswer() { 
            return options[correctIndex];
        }

        public override Question Clone()
        {
            return new MultipleChoiceQuestion(this);
        }
    }
}
