using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudyQuizApp.Helpers;
using StudyQuizApp.Models;
using StudyQuizApp.Services;
using StudyQuizApp.ViewModels.DisplayContent;

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
                var q = quizManager.RetrieveQuestion(SelectedIndex);
                if (q is QualitativeQuestion qualitative)
                {
                    DisplayContent = new QualitativeQuestionDetailContent
                    {
                        QuestionText = qualitative.QuestionText,
                        Answer = qualitative.Answer
                    };
                }
                else if (q is MultipleChoiceQuestion mcq)
                {
                    var options = mcq.Options.Select((opt, index) => new OptionViewModel
                    {
                        Text = opt,
                        IsCorrect = index == mcq.CorrectIndex
                    }).ToList();

                    DisplayContent = new MultipleChoiceQuestionDetailContent
                    {
                        QuestionText = mcq.QuestionText,
                        FormattedOptions = options
                    };
                }
            }
        }

        public ICommand ClearSelectionCommand { get; }

        public bool CanEditOrDelete => SelectedIndex >= 0;

        public MainViewModel()
        {
            UpdateQuestionList();

            ClearSelectionCommand = new RelayCommand(
                _ => ClearSelection(),
                _ => CanEditOrDelete
            );

            DisplayContent = new InstructionContent
            {
                Heading = "Welcome to StudyQuizApp!",
                Subheading = "Create questions or load an existing CSV file, and run the quiz to rehearse.",
                Instructions = new List<string>
                {
                    "Create questions manually by clicking the Add buttons in the upper left corner",
                    "Select a question in the list to view its details here",
                    "Upload existing questions via the \"File\" menu option",
                    "Click \"Run quiz\" to start rehearsing"
                }
            };
        }

        private object _displayContent;
        public object DisplayContent
        {
            get => _displayContent;
            set
            {
                _displayContent = value;
                OnPropertyChanged();
            }
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

        private void ClearSelection()
        {
            SelectedIndex = -1;
            DisplayContent = new InstructionContent
            {
                Heading = "You're using StudyQuizApp!",
                Subheading = "Create questions or load an existing CSV file, and run the quiz to rehearse.",
                Instructions = new List<string>
        {
            "Create questions manually by clicking the Add buttons in the upper left corner",
            "Select a question in the list to view its details here",
            "Upload existing questions via the \"File\" menu option",
            "Click \"Run quiz\" to start rehearsing"
        }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
