using System.Text.Json;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    /// <summary>
    /// AJAX updates one section of LK_Data, leaving the rest untouched.
    /// </summary>
    public class MVC_LK_SectionUpdate
    {
        public string Section { get; set; } = "";
        public JsonElement Value { get; set; }
    }
}
