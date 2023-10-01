using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Extensions;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
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

            for (int i = 0; i < model.TestQuestions.Count; i++) // Проверка тестов
            {
                if (model.TestQuestions[i].UserAnswer == model.TestQuestions[i].InnerAnswer)
                {
                    grade = +1.5f;
                    tasksCorrectness.Add(true);
                    continue;
                }
                tasksCorrectness.Add(false);
            }

            for (int i = 0; i < model.OpenQuestions.Count; i++) // Проверка открытых вопросов
            {
                for (int j = 0; j < model.OpenQuestions[i].InnerAnswers.Count; j++)
                {
                    if (model.OpenQuestions[i].InnerAnswers[j] == model.OpenQuestions[i].UserAnswer)
                    {
                        tasksCorrectness.Add(true);
                        grade = +3.5f;
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
            return new LessonPassViewModel
            {
                LessonId = lessonId,
                TestQuestions = (from question in questions
                                       where question.LessonId == lessonId
                                       where question.Type == TaskType.Test
                                       orderby question.Number
                                       select new TestQuestionViewModel
                                       {
                                           Number = question.Number,
                                           DisplayQuestion = question.DisplayQuestion,
                                           VariantsOfAnswer = (from testVariant in testVariants
                                                               where testVariant.QuestionId == question.Id
                                                               select testVariant).ToList(),
                                           InnerAnswer = question.Answer,
                                           RightPageAnswer = (from testVariant in testVariants
                                                              where testVariant.QuestionId == question.Id && testVariant.IsRight
                                                              select testVariant.Content).First(),
                                       }).ToList(),
                OpenQuestions = QuestionExstension.CreateDifferentValuesList((from question in questions
                                                                              where question.Type == TaskType.Open && question.LessonId == lessonId
                                                                              select question).ToList())
            };
        }
    }
}
