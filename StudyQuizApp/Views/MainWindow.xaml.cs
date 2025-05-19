using System.Text;
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
            MessageBox.Show("Add Qualitative Question button clicked!");

        }
    }
}