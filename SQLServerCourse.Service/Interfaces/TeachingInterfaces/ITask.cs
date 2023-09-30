using SQLServerCourse.Domain.ViewModels.Lesson;
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
                if (model.UserAnswers[i] == model.TestQuestions[i].InnerAnswer)
                {
                    grade = +1.5f;
                    tasksCorrectness.Add(true);
                }
                tasksCorrectness.Add(false);
            }

            for (int i = 0; i < model.OpenQuestions.Count; i++) // Проверка открытых вопросов
            {
                for (int j = 0; j < model.OpenQuestions[i].Answers.Count; j++)
                {
                    if (model.OpenQuestions[i].Answers[j] == model.UserAnswers[i])
                    {
                        tasksCorrectness.Add(true);
                        grade = +3.5f;
                    }
                }
                tasksCorrectness.Add(false);
            }

            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }
    }
}
