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
using StudyQuizApp.Models;

namespace StudyQuizApp.Views
{
    /// <summary>
    /// Interaction logic for QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {

        // Fields
        private Question? questionResult;

        public QuestionWindow(QuestionType type)
        {
            InitializeComponent();
            InitializeGUIForType(type);
        }

        public QuestionWindow(Question question)
        {
            InitializeComponent();
            InitializeGUIForType(question.Type);
            PopulateFields(question);
        }

        // Properties
        public Question QuestionResult { get => questionResult; }

        private void InitializeGUIForType(QuestionType type) 
        {
         if (type == QuestionType.Qualitative)
            {
                // Update header
                titleTextBlock.Text = "Add new Q&A style question:";

                // Add label and textbox for "Answer"
                // Create and add label
                Label answerLabel = new Label
                {
                    Content = "Answer:"
                };
                dynamicFieldsPanel.Children.Add(answerLabel);

                // Create and add multiline textbox
                TextBox answerTextBox = new TextBox
                {
                    Name = "answerTextBox",  // Optional, only needed if you need to access it later
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 100,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                dynamicFieldsPanel.Children.Add(answerTextBox);
            } else
            {

                // Update header
                titleTextBlock.Text = "Add new Q&A style question:";

                // Create 4 option textboxes
                for (int i = 0; i < 4; i++)
                {

                    Label optionLabel = new Label { Content = $"Option {i + 1}:", Name = $"optionsLbl{i + 1}"};
                    dynamicFieldsPanel.Children.Add(optionLabel);

                    TextBox optionTextBox = new TextBox
                    {
                        AcceptsReturn = true,
                        TextWrapping = TextWrapping.Wrap,
                        Height = 35,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        Name = $"optionTextBox{i + 1}"
                    };
                    dynamicFieldsPanel.Children.Add(optionTextBox);
                }

                // Label + ComboBox for correct answer
                StackPanel correctAnswerRow = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 10, 0, 10)
                };

                Label correctAnswerLabel = new Label
                {
                    Content = "Correct Answer:"
                };
                correctAnswerRow.Children.Add(correctAnswerLabel);

                ComboBox correctAnswerComboBox = new ComboBox
                {
                    ItemsSource = new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" },
                    SelectedIndex = 0,  // Default selection
                    Margin = new Thickness(0, 0, 0, 10)
                };
                correctAnswerRow.Children.Add(correctAnswerComboBox);

                // Add the row to the dynamic panel
                dynamicFieldsPanel.Children.Add(correctAnswerRow);
            }
        }

        private void PopulateFields(Question question)
        {
            // populate fields
        }
    }
}
