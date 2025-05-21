using System.Windows;
using System.Windows.Controls;
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
        private QuestionType currentType;

        public QuestionWindow(QuestionType type)
        {
            InitializeComponent();
            currentType = type;
            InitializeGUI();
        }

        public QuestionWindow(Question question)
        {
            InitializeComponent();
            currentType = question.Type;
            InitializeGUI();
            PopulateFields(question);
        }

        // Properties
        public Question QuestionResult { get => questionResult; }

        private void InitializeGUI() 
        {
         if (currentType == QuestionType.Qualitative)
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

            titleTextBlock.Text = "Edit question:";

            // populate fields
            questionTextBox.Text = question.QuestionText;

            if (currentType == QuestionType.Qualitative)
            {
                // populate fields answer textbox
                TextBox? answerBox = dynamicFieldsPanel.Children
                    .OfType<TextBox>()
                    .FirstOrDefault();

                if (answerBox != null && question is QualitativeQuestion qq)
                {
                    answerBox.Text = qq.Answer;
                }

            }
            else
            {
                // populate fields options and combobox
                // Fill in the 4 option TextBoxes
                var optionBoxes = dynamicFieldsPanel.Children
                    .OfType<TextBox>()
                    .Take(4)  // Assuming they're added in order
                    .ToList();

                if (question is MultipleChoiceQuestion mcq)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        optionBoxes[i].Text = mcq.Options[i];
                    }

                    // Find the ComboBox inside the StackPanel for correct answer
                    ComboBox? correctCombo = dynamicFieldsPanel.Children
                        .OfType<StackPanel>()
                        .SelectMany(sp => sp.Children.OfType<ComboBox>())
                        .FirstOrDefault();

                    if (correctCombo != null)
                    {
                        correctCombo.SelectedIndex = mcq.CorrectIndex;
                    }
                }
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentType == QuestionType.Qualitative)
            {
                try
                {
                    // Get answer textbox (assumes it's the second child in dynamicFieldsPanel)
                    TextBox answerBox = dynamicFieldsPanel.Children
                        .OfType<TextBox>()
                        .FirstOrDefault();

                    if (answerBox == null)
                        throw new Exception("Answer field not found.");

                    questionResult = new QualitativeQuestion(questionTextBox.Text, answerBox.Text);
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error:\n{ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                
            } else
            {
                try
                {
                    // Collect all 4 option textboxes
                    var optionBoxes = dynamicFieldsPanel.Children
                        .OfType<TextBox>()
                        .ToList();

                    if (optionBoxes.Count < 4)
                        throw new Exception("Not all 4 options are filled.");

                    string[] options = optionBoxes.Take(4).Select(tb => tb.Text).ToArray();

                    // Get ComboBox for correct index (assumes last child is StackPanel with ComboBox)
                    ComboBox combo = dynamicFieldsPanel.Children
                        .OfType<StackPanel>()
                        .SelectMany(sp => sp.Children.OfType<ComboBox>())
                        .FirstOrDefault();

                    if (combo == null)
                        throw new Exception("Could not read 'correct answer' selection.");

                    int correctIndex = combo.SelectedIndex;

                    questionResult = new MultipleChoiceQuestion(questionTextBox.Text, options, correctIndex);
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error:\n{ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

            }
        }
    }
}
