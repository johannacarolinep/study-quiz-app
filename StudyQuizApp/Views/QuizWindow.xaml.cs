using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StudyQuizApp.Services;
using StudyQuizApp.ViewModels;

namespace StudyQuizApp.Views
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly QuizViewModel viewModel;

        public QuizWindow(QuizManager quizManager)
        {
            InitializeComponent();
            viewModel = new QuizViewModel(quizManager);
            DataContext = viewModel;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is QuizViewModel vm && sender is RadioButton rb && rb.Tag is string option)
            {
                vm.SelectedOption = option;
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
