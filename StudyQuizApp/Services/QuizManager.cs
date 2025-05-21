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

        public void UpdateQuestion(Question updatedQuestion, int index)
        {
            if (!CheckIndex(index)) {
                throw new Exception("Could not find the question to update.");
            }
            questionList[index] = updatedQuestion;
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
            if (index >= 0 && index <= questionList.Count) { return true; }
            return false;
        }
    }
}
