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

    }
}
