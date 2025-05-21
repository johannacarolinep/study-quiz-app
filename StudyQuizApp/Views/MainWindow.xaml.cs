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
                        "Sorry, something went wrong when adding the customer. Please try again.",
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
    }
}