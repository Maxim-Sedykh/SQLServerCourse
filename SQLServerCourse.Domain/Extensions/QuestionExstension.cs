using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
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
        public static List<QuestionViewModel> CreateDifferentValuesList(List<Question> questions, List<TestVariant> testVariants)
        {
            List<QuestionViewModel> resultList = new List<QuestionViewModel>();
            for (int i = 0, j = 0; i < questions.Count; i++)
            {
                if (i > 0)
                {
                    if (questions[i - 1].Number == questions[i].Number)
                    {
                        resultList[j - 1].InnerAnswers.Add(questions[i].Answer);
                        continue;
                    }
                    goto CreateViewModel;
                }

            CreateViewModel:
                resultList.Add(new QuestionViewModel
                {
                    Number = questions[i].Number,
                    DisplayQuestion = questions[i].DisplayQuestion,
                    QuestionType = questions[i].Type,
                    VariantsOfAnswer = questions[i].Type == TaskType.Test ? (from testVariant in testVariants
                                                                             where testVariant.QuestionId == questions[i].Id
                                                                             select testVariant).ToList() : null,
                    InnerAnswers = new List<string> { questions[i].Answer },
                    RightPageAnswer = questions[i].Type == TaskType.Open ? questions[i].Answer : (from testVariant in testVariants
                                                                                                  where testVariant.QuestionId == questions[i].Id
                                                                                                  where testVariant.IsRight
                                                                                                  select testVariant.Content).First(),
                });
                j++;
            }

            return resultList;
        }
    }
}
