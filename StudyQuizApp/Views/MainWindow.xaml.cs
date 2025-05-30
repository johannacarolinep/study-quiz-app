using StudyQuizApp.Helpers;
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

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                DefaultExt = ".csv",
                Title = "Save Questions"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                if (!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Please select a valid .csv file.", "Invalid File Type", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {

                    FileManager fileManager = new FileManager();
                    fileManager.SaveQuestionsToFile(filePath, viewModel.QuestionList);

                    MessageBox.Show("Questions saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
