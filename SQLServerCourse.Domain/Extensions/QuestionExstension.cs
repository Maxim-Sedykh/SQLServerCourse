using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Extensions
{
    public static class QuestionExstension
    {
        public static List<OpenQuestionViewModel> CreateDifferentValuesList(this List<Question> questions)
        {
            List<OpenQuestionViewModel> resultList = new List<OpenQuestionViewModel>();
            for (int i = 0, j = 0; i < questions.Count; i++)
            {
                if (i > 0) 
                {
                    if (questions[i - 1].Number == questions[i].Number)
                    {
                        resultList[j - 1].Answers.Add(questions[i].Answer);
                        continue;
                    }
                    goto CreateViewModel;
                }

            CreateViewModel:
                resultList.Add(new OpenQuestionViewModel
                {
                    Number = questions[i].Number,
                    DisplayQuestion = questions[i].DisplayQuestion,
                    Answers = new List<string> { questions[i].Answer },
                });
                j++;
            }
            return resultList;
        }
    }
}
