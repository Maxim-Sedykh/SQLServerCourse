﻿using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Helpers;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Lesson
{
    public class LessonPassViewModel
    {
        public int LessonId { get; set; }

        public List<QuestionViewModel> Questions { get; set; }

        public List<bool>? TasksCorrectness { get; set; }

        [MaxLength(500, ErrorMessage = "Ответ должен быть меньше 500 символов!")]
        public List<string>? Answers { get; set; }

        //public PracticalTaskEvaluationHelper[]? PracticalTasks { get; set; }
    }
}
