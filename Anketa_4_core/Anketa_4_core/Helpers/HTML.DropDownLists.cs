using Anketa_4_core.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace Anketa_4_core.Helpers
{
    public static class HTML_DropDownLists_Extensions
    {
        /// <summary>
        /// Рисует <select> c филиалами из базы
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedID">Выбранный заранее филиал филиал</param>
        /// <param name="otherAttributes">остальные аттрибуты (при необходимости)</param>
        /// <returns><select> c филиалами из базы</returns>
        public static IHtmlContent DDL_Filials(this IHtmlHelper htmlHelper, 
                                        string[] classes, 
                                        string name, 
                                        int selectedID=-1, 
                                        string otherAttributes="")
        {
            var sb = new StringBuilder($"<select name=\"{name}\" class=\"{string.Join(' ', classes)}\" {otherAttributes}>");
            using (var context = new AnketaContext())
            {
                foreach(var item in context.Filials) 
                {
                    string selected = item.ID == selectedID ? "selected" : "";

                    sb.Append($"<option value=\"{item.ID}\" {selected}>{item.FilialName}</option>");
                }
            }
            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }


        /// <summary>
        /// Рисует <select> c реальными названиями филиалов
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedID">Выбранный заранее филиал филиал</param>
        /// <param name="otherAttributes">остальные аттрибуты (при необходимости)</param>
        /// <returns><select> c реальными названиями филиалов</returns>
        public static IHtmlContent DDL_Filials(this IHtmlHelper htmlHelper, 
                                        string[] classes, 
                                        string name, 
                                        Dictionary<string, string> realFiilals, 
                                        int selectedID = -1, 
                                        string otherAttributes = "")
        {
            var sb = new StringBuilder($"<select name=\"{name}\" class=\"{string.Join(' ', classes)}\" {otherAttributes}>");
            using (var context = new AnketaContext())
            {
                foreach (var item in context.Filials)
                {
                    string selected = item.ID == selectedID ? "selected" : "";
                    string? realName;
                    realFiilals.TryGetValue(item.FilialName, out realName);
                    realName = realName ?? item.FilialName;
                    sb.Append($"<option value=\"{item.ID}\" {selected}>{realName}</option>");
                }
            }
            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Рисует <select> c филиалами из базы
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedID">Выбранный заранее филиал филиал</param>
        /// <param name="otherAttributes">остальные аттрибуты (при необходимости)</param>
        /// <returns><select> c филиалами из базы</returns>
        public static IHtmlContent DDL_Reservs(this IHtmlHelper htmlHelper,
                                        string[] classes,
                                        string name, 
                                        int selectedID = -1,
                                        string otherAttributes = "")
        {
            var sb = new StringBuilder($"<select name=\"{name}\" class=\"{string.Join(' ', classes)}\" {otherAttributes}>");
            using (var context = new AnketaContext())
            {
                foreach (var item in context.ReservLevels)
                {
                    string selected = item.ID == selectedID ? "selected" : "";

                    sb.Append($"<option value=\"{item.ID}\" {selected}>{item.ReservLevelName}</option>");
                }
            }
            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }
    }
}
