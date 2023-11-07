using Anketa_4_core.Data;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Helpers
{
    public static class AccountHelper
    {
        public static int LastAccesForTestableByLoginUserName(string UserName)
        {
            using (var context = new AnketaContext())
            {
                int testableID = context.TestableUsers.Include(u=>u.testable).First(t=>t.UserName == UserName).testable.ID;
                return LastAccesForTestableByTestableID(testableID);
            }

        }
        public static int LastAccesForTestableByCode(string Code )
        {
            using (var context = new AnketaContext())
            {
                int testableID = context.Testables.First(t => t.Code == Code).ID;
                return LastAccesForTestableByTestableID(testableID);
            }
        }

        private static int LastAccesForTestableByTestableID(int ID)
        {
            using (var context = new AnketaContext())
            {
                try
                {
                     return context.AccessForTestables.Include(t => t.Testable).Last(aft => aft.Testable.ID == ID).ID;
                }
                catch (Exception ex)
                {
                    //TODO журналировать
                    return -1;
                }                 
             }
        }
    }
}
