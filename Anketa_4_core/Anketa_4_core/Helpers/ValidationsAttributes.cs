using Anketa_4_core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UniqueCode : ValidationAttribute
    {
        public UniqueCode()
        {
            ErrorMessage = "Такой код уже существует";
        }

        public override bool IsValid(object? value)
        {
            string? code = value as string;
            bool result = true;
            using (var context = new AnketaContext())
            {
                if (code != null)
                {
                    if (context.Testables.FirstOrDefault(t => t.Code==code) != null)
                        return false;
                }
            }
            return true;
        }

    }

}
