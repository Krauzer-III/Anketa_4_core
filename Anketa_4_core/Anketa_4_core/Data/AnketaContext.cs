using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Anketa_4_core.Models.AnketaModels.MVC_Models;

namespace Anketa_4_core.Data
{
    public class AnketaContext : DbContext
    {
        public DbSet<Testable> Testables { get; set; }
        public DbSet<Filial> Filials { get; set; }
        public DbSet<ReservLevel> ReservLevels { get; set; }
        public DbSet<AccessForTestable> AccessForTestables { get; set; }
        public DbSet<TestPeriod> TestPeriods { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Test360> Test360 { get; set; }
        public DbSet<MotivationTest> MotivationTests { get; set; }
        public DbSet<RespondentRole> RespondentRoles { get; set; }
        public DbSet<Comp_Block> Comp_Blocks { get; set; }
        public DbSet<Comp_Question> Comp_Questions { get; set; }
        public DbSet<Comp_Answer> Comp_Answers { get; set; }
        public DbSet<Comp_TestableAnswer> Comp_TestableAnswers { get; set; }
        public DbSet<KT_Main> KT_Mains { get; set; }
        public DbSet<KT_Question> KT_Questions { get; set; }
        public DbSet<KT_Answer> KT_Answers { get; set; }
        public DbSet<KT_Category> KT_Categories { get; set; }
        public DbSet<KT_Mark> KT_Marks { get; set; }
        public DbSet<KT_KeyForCategory> KT_Keys { get; set; }
        public DbSet<KT_TestableAnswers> KT_TestableAnswers { get; set; }
        public DbSet<TestableUser> TestableUsers { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        public AnketaContext(DbContextOptions<AnketaContext> options)
            : base(options) { }

        public AnketaContext()
            : base() { }

        public DbSet<Anketa_4_core.Models.AnketaModels.MVC_Models.MVC_TestableView>? MVC_TestableView { get; set; }

    }

}
