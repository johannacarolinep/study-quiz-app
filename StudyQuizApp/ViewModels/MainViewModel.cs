using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using StudyQuizApp.Helpers;
using StudyQuizApp.Models;
using StudyQuizApp.Services;
using StudyQuizApp.ViewModels.DisplayContent;
using StudyQuizApp.Views;

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
                UpdateDisplayContent();
            }
        }

        public ICommand ClearSelectionCommand { get; }
        public ICommand EditQuestionCommand { get; }
        public ICommand DeleteQuestionCommand { get; }

        public bool CanEditOrDelete => SelectedIndex >= 0;

        public MainViewModel()
        {
            UpdateQuestionList();

            ClearSelectionCommand = new RelayCommand(
                _ => ClearSelection(),
                _ => CanEditOrDelete
            );

            EditQuestionCommand = new RelayCommand(
                _ => OnEditQuestion(),
                _ => CanEditOrDelete
            );

            DeleteQuestionCommand = new RelayCommand(
                _ => OnDeleteQuestion(),
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

        private void UpdateDisplayContent()
        {
            if (SelectedIndex == -1)
            {
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
                return;
            }

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

        public void AddQuestion(Question question)
        {
            quizManager.AddQuestion(question);
            UpdateQuestionList();
        }

        public Question RetrieveSelectedQuestion()
        {
            return quizManager.RetrieveQuestion(SelectedIndex);
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


        private void OnEditQuestion()
        {
            if (SelectedIndex < 0)
            {
                MessageBox.Show("Please select a question to edit.", "No selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Question selectedQ = RetrieveSelectedQuestion();
            if (selectedQ == null)
            {
                MessageBox.Show("Failed to retrieve question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            QuestionWindow editWindow = new QuestionWindow(selectedQ);
            bool? result = editWindow.ShowDialog();

            if (result == true && editWindow.QuestionResult is Question updatedQ)
            {
                try
                {
                    quizManager.UpdateQuestion(updatedQ, SelectedIndex);
                    int currentIndex = SelectedIndex;
                    UpdateQuestionList();
                    SelectedIndex = currentIndex;
                    MessageBox.Show("Question updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Failed to update question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (result == true)
            {
                MessageBox.Show("Failed to update question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnDeleteQuestion()
        {
            if (SelectedIndex < 0)
            {
                MessageBox.Show("Please first select which question to delete.", "No selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            string questionText = QuestionStrings[SelectedIndex];

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete this question?\n\n\"{questionText}\"",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    quizManager.DeleteQuestion(SelectedIndex);
                    UpdateQuestionList();
                    MessageBox.Show("Question deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
