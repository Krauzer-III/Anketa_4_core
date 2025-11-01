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
        /// <param name="selectedID">Выбранный заранее филиал</param>
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
        /// <param name="selectedID">Выбранный заранее филиал</param>
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
        /// Рисует <select> c уровнями резерва
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedID">Выбранный заранее филиал</param>
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


        /// <summary>
        /// Рисует <select> c ролями респондентов
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedID">Выбранная заранее роль респондента</param>
        /// <param name="otherAttributes">остальные аттрибуты (при необходимости)</param>
        /// <returns><select> c филиалами из базы</returns>
        public static IHtmlContent DDL_RespondentRoles(this IHtmlHelper htmlHelper,
                                        string[] classes,
                                        string name,
                                        int selectedID = -1,
                                        string otherAttributes = "")
        {
            var sb = new StringBuilder($"<select name=\"{name}\" class=\"{string.Join(' ', classes)}\" {otherAttributes}>");
            using (var context = new AnketaContext())
            {
                foreach (var item in context.RespondentRoles)
                {
                    string selected = item.ID == selectedID ? "selected" : "";

                    sb.Append($"<option value=\"{item.ID}\" {selected}>{item.Name}</option>");
                }
            }
            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }



        /// <summary>
        /// Рисует select c филиалами с чекбоксами
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="otherAttributes">остальные аттрибуты (при необходимости)</param>
        /// <returns><select> c филиалами из базы</returns>
        public static IHtmlContent DDL_Checkbox_Filial(this IHtmlHelper htmlHelper,
                                        string[] classes,
                                        string name)
        {
            var sb = new StringBuilder($"<div class=\"dropdown\">" +
                $"<button data-mdb-button-init data-mdb-ripple-init data-mdb-dropdown-init class=\"btn btn-primary dropdown-toggle\" type=\"button\" id=\"{name}\"data-mdb-toggle=\"dropdown\" aria-expanded=\"false\">" +
                $"Филиалы" +
                $"</button>" +
                $"<ul class=\"dropdown-menu\" aria-labelledby=\"{name}\">");
            using (var context = new AnketaContext())
            {
                int i = 1;
                foreach (var item in context.Filials)
                {
                    sb.Append($"<li>" +
                        $"<a class=\"dropdown-item\" href=\"#\">" +
                        $"<div class=\"form-check\">" +
                        $"<input class=\"form-check-input\" type=\"checkbox\" value=\"{item.ID}\" id=\"{name + i.ToString()}\" name=\"{name}\" />" +
                        $"<label class=\"form-check-label\" for=\"{name + i++.ToString()}\">{item.FilialName}</label>" +
                        $"</div>" +
                        $"</a>" +
                        $"</li>");
                }
            }
            sb.AppendLine("</ul></div>");
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Рисует &lt;select multiple&gt; с реальными названиями TestPeriod (GroupName)
        /// </summary>
        /// <param name="classes">CSS классы</param>
        /// <param name="name">Атрибут name</param>
        /// <param name="selectedIds">Заранее выбранные ID периодов</param>
        /// <param name="otherAttributes">Другие атрибуты</param>
        /// <returns>&lt;select&gt; с вариантами периодов</returns>
        public static IHtmlContent DDL_TestPeriods(this IHtmlHelper htmlHelper,
                                                   string[] classes,
                                                   string name,
                                                   IEnumerable<int>? selectedIds = null,
                                                   string otherAttributes = "")
        {
            selectedIds ??= Enumerable.Empty<int>();
            var sel = new HashSet<int>(selectedIds);
            var sb = new StringBuilder($"<select name=\"{name}\" class=\"{string.Join(' ', classes)}\" multiple {otherAttributes}>");
            using (var context = new AnketaContext())
            {
                foreach (var item in context.TestPeriods.OrderBy(x => x.GroupName))
                {
                    string selected = sel.Contains(item.ID) ? "selected" : "";
                    sb.Append($"<option value=\"{item.ID}\" {selected}>{item.GroupName}</option>");
                }
            }
            sb.AppendLine("</select>");
            return new HtmlString(sb.ToString());
        }
    }
}
