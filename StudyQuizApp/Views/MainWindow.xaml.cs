using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudyQuizApp.Models;
using StudyQuizApp.Services;

namespace StudyQuizApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fields
        private QuizManager quizManager = new QuizManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeGUI()
        {
            EnableEditAndDelete(false);
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
            QuestionWindow questionWindow = new QuestionWindow(type);

            bool? result = questionWindow.ShowDialog();

            if (result == true)
            {
                Question newQuestion = questionWindow.QuestionResult;

                if (newQuestion != null)
                {
                    // Add question and update UI
                    quizManager.AddQuestion(newQuestion);
                    UpdateQListBox();

                    MessageBox.Show(
                        "Question was successfully created!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Sorry, something went wrong when adding the question. Please try again.",
                        "Unexpected failure",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }

            }
        }

        private void UpdateQListBox()
        {
            questionsListBox.ItemsSource = quizManager.GetQuestionStrings();
        }

        private void qListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable edit and delete buttons
            EnableEditAndDelete(true);

            // To be implemented - display question details
        }

        private void EnableEditAndDelete(bool shouldBeEnabled)
        {
            editQBtn.IsEnabled = shouldBeEnabled;
            deleteQBtn.IsEnabled = shouldBeEnabled;
        }

        private void editQBtn_Click(object sender, RoutedEventArgs e)
        {
            // Double check a question is selected
            if (questionsListBox.SelectedItem == null) {
                MessageBox.Show(
                    "Please select the question you wish to edit from the list first.",
                    "No question selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Retreieve a copy of the question
            Question qToEdit = quizManager.RetrieveQuestion(questionsListBox.SelectedIndex);

            if (qToEdit == null) {
                MessageBox.Show(
                    "Sorry, we failed to retrieve the question. Please try again.",
                    "Failure to retrieve question",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                 return;
            }

            // open an edit form (questionform)
            QuestionWindow questionWindow = new QuestionWindow(qToEdit);

            bool? result = questionWindow.ShowDialog();

            // check results - if ok, update question in quizmanager
            if (result == true)
            {
                Question updatedQuestion = questionWindow.QuestionResult;

                if (updatedQuestion != null)
                {
                    // Add question and update UI
                    try
                    {
                        quizManager.UpdateQuestion(updatedQuestion, questionsListBox.SelectedIndex);
                        questionsListBox.SelectedIndex = 0;
                        EnableEditAndDelete(false);
                        UpdateQListBox();
                        MessageBox.Show(
                            "Question was successfully updated!",
                            "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex) {
                        MessageBox.Show(
                            $"Error: {ex.Message}",
                            "Unexpected failure",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Sorry, something went wrong when updating the question. Please try again.",
                        "Unexpected failure",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }
    }
}