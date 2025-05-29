using System;
using System.ComponentModel;
using System.Windows;
using StudyQuizApp.Helpers;
using StudyQuizApp.Models;
using StudyQuizApp.Services;
using StudyQuizApp.ViewModels.Enums;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace StudyQuizApp.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly QuizManager quizManager;
        private int currentQuestionNumber;
        private int totalQuestions;
        private int correctCount;
        private double percent;
        private QuizState quizState;
        private Question currentQuestion;
        private string selectedOption;
        private bool? isCorrect;
        private string correctAnswerText;
        private ObservableCollection<string> finishedAttempts = new();
        private int incorrectCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand RevealAnswerCommand { get; }
        public RelayCommand RegisterAnswerCommand { get; }

        public QuizViewModel(QuizManager quizManager)
        {
            this.quizManager = quizManager;
            quizManager.InitiateQuiz();
            UpdateStats();

            RevealAnswerCommand = new RelayCommand(_ => OnRevealAnswer(), _ => CanRevealAnswer());
            RegisterAnswerCommand = new RelayCommand(_ => OnRegisterAnswer(), _ => QuizState == QuizState.RevealingAnswer);

            RunQuizAttempt();
        }

        public int CurrentQuestionNumber
        {
            get => currentQuestionNumber;
            set
            { 
                currentQuestionNumber = value;
                OnPropertyChanged(nameof(CurrentQuestionNumber));
                OnPropertyChanged(nameof(QuestionsAnswered));
            }
        }

        public int TotalQuestions
        {
            get => totalQuestions;
            set { totalQuestions = value; OnPropertyChanged(nameof(TotalQuestions)); }
        }

        public int CorrectCount
        {
            get => correctCount;
            set { correctCount = value; OnPropertyChanged(nameof(CorrectCount)); }
        }

        public int QuestionsAnswered => CurrentQuestionNumber > 0 ? CurrentQuestionNumber - 1 : 0;

        public double Percent
        {
            get => percent;
            set { percent = value; OnPropertyChanged(nameof(Percent)); }
        }

        
        public int IncorrectCount
        {
            get => incorrectCount;
            set { 
                incorrectCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMistakes));
                OnPropertyChanged(nameof(HasPerfectScore));
            }
        }

        public bool HasMistakes => IncorrectCount > 0;

        public bool HasPerfectScore => IncorrectCount == 0;


        public bool IsLastQuestion => quizManager.IsLastQuestion();

        public string NextOrFinishText => IsLastQuestion ? "Finish" : "Next";

        public ObservableCollection<string> FinishedAttempts
        {
            get => finishedAttempts;
            set { finishedAttempts = value; OnPropertyChanged(); }
        }


        public Question CurrentQuestion
        {
            get => currentQuestion;
            set
            {
                currentQuestion = value;
                OnPropertyChanged(nameof(CurrentQuestion));
                OnPropertyChanged(nameof(IsMultipleChoice));
                OnPropertyChanged(nameof(IsQualitative));
                OnPropertyChanged(nameof(IsLastQuestion));
                OnPropertyChanged(nameof(NextOrFinishText));
            }
        }


        public QuizState QuizState
        {
            get => quizState;
            set
            {
                quizState = value;
                OnPropertyChanged(nameof(QuizState));
                OnPropertyChanged(nameof(IsShowingQuestion));
                OnPropertyChanged(nameof(IsRevealingAnswer));
                OnPropertyChanged(nameof(ShowAnswerDetails));
                OnPropertyChanged(nameof(IsQuizFinished));
                RevealAnswerCommand.RaiseCanExecuteChanged();
                RegisterAnswerCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(NextOrFinishText));
            }
        }

        public string SelectedOption
        {
            get => selectedOption;
            set
            {
                selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
                // Optional if any dynamic update is needed
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        public bool? IsCorrect
        {
            get => isCorrect;
            set { isCorrect = value; OnPropertyChanged(nameof(IsCorrect)); }
        }

        public string CorrectAnswerText
        {
            get => correctAnswerText;
            set { correctAnswerText = value; OnPropertyChanged(nameof(CorrectAnswerText)); }
        }

        public bool ShowAnswerDetails => QuizState == QuizState.RevealingAnswer;
        public bool IsShowingQuestion => QuizState == QuizState.ShowingQuestion;
        public bool IsRevealingAnswer => QuizState == QuizState.RevealingAnswer;

        public bool IsQuizFinished => QuizState == QuizState.QuizFinished;
        public bool IsMultipleChoice => CurrentQuestion is MultipleChoiceQuestion;
        public bool IsQualitative => CurrentQuestion is QualitativeQuestion;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var name in propertyNames)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void UpdateStats()
        {
            CurrentQuestionNumber = quizManager.CurrentIndex + 1;
            TotalQuestions = quizManager.CurrentAttempt.QuestionsCount;
            CorrectCount = quizManager.CurrentAttempt.CorrectCount;
            Percent = quizManager.CurrentAttempt.GetRelativePoint(QuestionsAnswered) * 100;
        }

        private void RunQuizAttempt()
        {
            CurrentQuestion = quizManager.GetCurrentQuestion();
            QuizState = QuizState.ShowingQuestion;
        }

        private void OnRevealAnswer()
        {
            if (CurrentQuestion is MultipleChoiceQuestion mcq)
            {
                if (string.IsNullOrEmpty(SelectedOption))
                {
                    MessageBox.Show("Please select an answer before revealing.", "No Answer Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                IsCorrect = SelectedOption == mcq.GetCorrectAnswer();
                CorrectAnswerText = $"Correct answer: {mcq.GetCorrectAnswer()}";
            }
            else if (CurrentQuestion is QualitativeQuestion q)
            {
                CorrectAnswerText = $"Suggested answer: {q.GetCorrectAnswer()}";
                IsCorrect = false;
            }

            QuizState = QuizState.RevealingAnswer;
        }

        private bool CanRevealAnswer()
        {
            return QuizState == QuizState.ShowingQuestion;
        }

        private void OnRegisterAnswer()
        {
            bool wasCorrect = false;

            if (CurrentQuestion is MultipleChoiceQuestion)
            {
                wasCorrect = IsCorrect == true;
            }
            else if (CurrentQuestion is QualitativeQuestion)
            {
                wasCorrect = IsCorrect == true;
            }

            if (wasCorrect)
            {
                quizManager.MarkCorrect();
                CorrectCount++;
                
            } else
            {
                quizManager.MarkIncorrect();
            }

            Percent = quizManager.CurrentAttempt.GetRelativePoint() * 100;

            LoadNextQOrFinishQuiz();
        }

        private void LoadNextQOrFinishQuiz()
        {
            if (!quizManager.IsLastQuestion())
            {
                quizManager.IncrementCurrentIndex();
                CurrentQuestion = quizManager.GetCurrentQuestion();
                UpdateStats();
                SelectedOption = null;
                IsCorrect = null;
                CorrectAnswerText = "";
                QuizState = QuizState.ShowingQuestion;
            }
            else
            {
                quizManager.FinishCurrentAttempt();
                CurrentQuestion = null;

                // Refresh finished attempts
                FinishedAttempts = new ObservableCollection<string>(quizManager.GetResultsSummaries());

                // check if there are incorrect questions
                IncorrectCount = quizManager.CountIncorrectIndices();

                QuizState = QuizState.QuizFinished;
            }
        }
    }
}