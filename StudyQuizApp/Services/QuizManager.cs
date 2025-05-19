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
        private List<Question> questionList;

        public QuizManager() { 
            questionList = new List<Question>();
        }

        // Methods
        public void AddQuestion(Question question) { 
        questionList.Add(question);
        }
    }
}
