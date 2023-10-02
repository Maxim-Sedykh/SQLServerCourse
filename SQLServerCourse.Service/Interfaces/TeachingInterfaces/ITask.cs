using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Extensions;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces.TeachingInterfaces
{
    public interface ITask
    {
        public static Tuple<float, List<bool>> CheckTasks(LessonPassViewModel model) // Проверка заданий тестового и открытого типа
        {
            float grade = 0;
            List<bool> tasksCorrectness = new List<bool>();

            for (int i = 0; i < model.Questions.Count; i++) // Проверка открытых вопросов
            {
                for (int j = 0; j < model.Questions[i].InnerAnswers.Count; j++)
                {
                    if (model.Questions[i].InnerAnswers[j] == model.Questions[i].UserAnswer)
                    {
                        tasksCorrectness.Add(true);
                        if (model.Questions[i].QuestionType == TaskType.Test)
                        {
                            grade = +1.5f;
                        }
                        else
                        {
                            grade = +3.5f;
                        }
                        goto LoopEnd;
                    }
                }
                tasksCorrectness.Add(false);
            LoopEnd: continue;
            }

            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }

        public static LessonPassViewModel GetQuestions(int lessonId, List<Question> questions, List<TestVariant> testVariants)
        {
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            for (int i = 0, j = 0; i < questions.Count; i++)
            {
                if (i > 0)
                {
                    if (questions[i - 1].Number == questions[i].Number)
                    {
                        questionViewModels[j - 1].InnerAnswers.Add(questions[i].Answer);
                        continue;
                    }
                    goto CreateViewModel;
                }

            CreateViewModel:
                questionViewModels.Add(new QuestionViewModel
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
            return new LessonPassViewModel
            {
                LessonId = lessonId,
                Questions = questionViewModels
            };
        }
    }
}
