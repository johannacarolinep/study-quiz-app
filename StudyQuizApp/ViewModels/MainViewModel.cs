using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using StudyQuizApp.Models;
using StudyQuizApp.Services;

namespace StudyQuizApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly QuizManager quizManager = new QuizManager();

        public ObservableCollection<string> QuestionStrings { get; set; } = new ObservableCollection<string>();

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanEditOrDelete));
            }
        }

        public bool CanEditOrDelete => SelectedIndex >= 0;

        public MainViewModel()
        {
            UpdateQuestionList();
        }

        public void UpdateQuestionList()
        {
            QuestionStrings.Clear();
            foreach (string qString in quizManager.GetQuestionStrings())
            {
                QuestionStrings.Add(qString);
            }
        }

        public void AddQuestion(Question question)
        {
            quizManager.AddQuestion(question);
            UpdateQuestionList();
        }

        public Question RetrieveSelectedQuestion()
        {
            return quizManager.RetrieveQuestion(SelectedIndex);
        }

        public void UpdateQuestion(Question updated)
        {
            quizManager.UpdateQuestion(updated, SelectedIndex);
            UpdateQuestionList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
