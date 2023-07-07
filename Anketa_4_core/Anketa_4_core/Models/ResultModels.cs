using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models
{
    public class ResultModels
    {
        [Key]
        public int ID { get; set; }
        [ty]
        public string JSONResult { get; set; }
        public TestType TypeResult { get; set; }
    }

    /// <summary>
    /// Тип теста: 360градусов, Кеттел, Мотивация, Компетенция, Компетенция 2
    /// </summary>
    public class TestType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
}
