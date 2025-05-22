using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyQuizApp.ViewModels.DisplayContent
{
    public class MultipleChoiceQuestionDetailContent : QuestionDetailContent
    {
        public List<OptionViewModel> FormattedOptions { get; set; }
    }
}
