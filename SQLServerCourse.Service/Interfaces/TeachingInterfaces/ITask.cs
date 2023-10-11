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
                            grade += 1.5f;
                        }
                        else
                        {
                            grade += 3.5f;
                        }
                        goto LoopEnd;
                    }
                }
                tasksCorrectness.Add(false);
            LoopEnd: continue;
            }

            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }

        public static LessonPassViewModel GetQuestions(Lesson currentLesson, List<Question> allQuestions, List<TestVariant> allTestVariants)
        {


            List<Question> lessonQuestions = (from question in allQuestions
                                              where question.LessonId == currentLesson.Id
                                              select question).ToList();
            List<TestVariant> lessonTestVariants = (from question in allQuestions
                                                    where question.LessonId == currentLesson.Id && question.Type == TaskType.Test
                                                    join testVariant in allTestVariants on question.Id equals testVariant.QuestionId
                                                    select testVariant).ToList();
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            for (int i = 0, j = 0; i < lessonQuestions.Count; i++)
            {
                if (i > 0)
                {
                    if (lessonQuestions[i - 1].Number == lessonQuestions[i].Number)
                    {
                        questionViewModels[j - 1].InnerAnswers.Add(lessonQuestions[i].Answer);

                        continue;
                    }
                    goto CreateViewModel;
                }

            CreateViewModel:
                questionViewModels.Add(new QuestionViewModel
                {
                    Number = lessonQuestions[i].Number,
                    DisplayQuestion = lessonQuestions[i].DisplayQuestion,
                    QuestionType = lessonQuestions[i].Type,
                    VariantsOfAnswer = lessonQuestions[i].Type == TaskType.Test ? (from testVariant in lessonTestVariants
                                                                                   where testVariant.QuestionId == lessonQuestions[i].Id
                                                                             select testVariant).ToList() : null,
                    InnerAnswers = new List<string> { lessonQuestions[i].Answer },
                    RightPageAnswer = lessonQuestions[i].Type == TaskType.Open ? lessonQuestions[i].Answer : (from testVariant in lessonTestVariants
                                                                                                              where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                                  where testVariant.IsRight
                                                                                                  select testVariant.Content).First(),
                    
                });
                j++;
            }
            return new LessonPassViewModel
            {
                LessonId = currentLesson.Id,
                Questions = questionViewModels,
                LessonType = currentLesson.LessonType
            };
        }
    }
}
