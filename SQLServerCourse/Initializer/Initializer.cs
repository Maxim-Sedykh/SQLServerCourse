using Microsoft.AspNetCore.Cors.Infrastructure;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Initializer
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<Lesson>, LessonRepository>();
            services.AddScoped<IBaseRepository<User>, UserRepository>();
            services.AddScoped<IBaseRepository<Question>, QuestionRepository>();
            services.AddScoped<IBaseRepository<TestVariant>, TestVariantRepository>();
            services.AddScoped<IBaseRepository<Review>, ReviewRepository>();
            services.AddScoped<IBaseRepository<PracticalTask>, PracticalTaskRepository>();
            services.AddScoped<IBaseRepository<LessonRecord>, LessonRecordRepository>();
            services.AddScoped<IBaseRepository<FrequentRemark>, FrequentRemarkRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IPersonalProfileService, PersonalProfileService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IBasicTeachingService, BasicTeachingService>();
            services.AddScoped<IFinalResultService, FinalResultService>();
            services.AddScoped<IHomeService, HomeService>();
        }
    }
}
