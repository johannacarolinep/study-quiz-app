using StudyQuizApp.Models;
using StudyQuizApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace StudyQuizApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        private void AddQualitativeQ_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateQuestionForm(QuestionType.Qualitative);
        }

        private void AddMultipleChoiceQ_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateQuestionForm(QuestionType.MultipleChoice);
        }

        private void OpenCreateQuestionForm(QuestionType type)
        {
            var questionWindow = new QuestionWindow(type);
            bool? result = questionWindow.ShowDialog();

            if (result == true && questionWindow.QuestionResult is Question newQuestion)
            {
                viewModel.AddQuestion(newQuestion);
                MessageBox.Show("Question created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (result == true)
            {
                MessageBox.Show("Failed to create question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void editQBtn_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a question to edit.", "No selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Question selectedQ = viewModel.RetrieveSelectedQuestion();
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
                    viewModel.UpdateQuestion(updatedQ);
                    MessageBox.Show("Question updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Failed to update question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else if (result == true) {
                MessageBox.Show("Failed to update question.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
