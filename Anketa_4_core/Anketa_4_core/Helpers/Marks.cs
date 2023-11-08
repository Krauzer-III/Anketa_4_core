using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.AspNetCore.Mvc;
using static Anketa_4_core.Data.AnketaModels.JSON_Motivation_Result;

namespace Anketa_4_core.Helpers
{
    public static class Marks
    {
        public static void AddDataToJSON<Test>(out JSON_ResultModel jsonModel, Test data)
        {

        } 

        private static void CompetentionMark(List<Comp_TestableAnswer> list, out List<JSON_Competention_Result> cr, out List<JSON_Competention_AnswerResult> ar)
        {
            cr = new List<JSON_Competention_Result>();
            ar = new List<JSON_Competention_AnswerResult>();
            using (var context = new AnketaContext())
            {
                foreach (var item in list)
                {

                }
            }
        }
    }
}
