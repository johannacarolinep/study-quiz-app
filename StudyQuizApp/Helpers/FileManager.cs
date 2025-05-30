using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyQuizApp.Models;

namespace StudyQuizApp.Helpers
{
    public class FileManager
    {
        public void SaveQuestionsToFile(string filePath, List<Question> questions)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var q in questions)
                {
                    if (q is MultipleChoiceQuestion mcq)
                    {
                        // Format: MultipleChoice;QuestionText;Opt0;Opt1;Opt2;Opt3;CorrectIndex
                        string line = string.Join(";", new string[]
                        {
                            "MultipleChoice",
                            mcq.QuestionText,
                            mcq.Options[0],
                            mcq.Options[1],
                            mcq.Options[2],
                            mcq.Options[3],
                            mcq.CorrectIndex.ToString()
                        });

                        writer.WriteLine(line);
                    }
                    else if (q is QualitativeQuestion qq)
                    {
                        // Format: Qualitative;QuestionText;Answer
                        string line = string.Join(";", new string[]
                        {
                            "Qualitative",
                            qq.QuestionText,
                            qq.Answer
                        });

                        writer.WriteLine(line);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unsupported question type.");
                    }
                }
            }
        }
    }
}
