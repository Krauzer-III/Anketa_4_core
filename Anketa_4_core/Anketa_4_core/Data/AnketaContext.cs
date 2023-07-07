using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Data
{
    public class AnketaContext : DbContext
    {
        public DbSet<Testable> Testables { get; set; }
        public DbSet<Filial> Filials { get; set; }
        public DbSet<ReservLevel> ReservLevels { get; set; }
        public DbSet<AccessForTestables> AccessForTestables { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Test360> Test360 { get; set; }
        public DbSet<MotivationTest> MotivationTests { get; set; }
        public DbSet<RespondentRole> RespondentRoles { get; set; }
        public DbSet<Comp_Blocks> Comp_Blocks { get; set; }
        public DbSet<Comp_Questions> Comp_Questions { get; set; }
        public DbSet<Comp_Answers> Comp_Answers { get; set; }
        public DbSet<Comp_Level> Comp_Levels { get; set; }
        public DbSet<Comp_TestableAnswers> Comp_TestableAnswers { get; set; }
        public DbSet<KT_Main> KT_Mains { get; set; }
        public DbSet<KT_Questions> KT_Questions { get; set; }
        public DbSet<KT_Answers> KT_Answers { get; set; }
        public DbSet<KT_Categories> KT_Categories { get; set; }
        public DbSet<KT_Levels> KT_Levels { get; set; }
        public DbSet<KT_Key> KT_Keys { get; set; }
        public DbSet<KT_TestableAnswers> KT_TestableAnswers { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<ResultModels> ResultModels { get; set; }
    }

}
