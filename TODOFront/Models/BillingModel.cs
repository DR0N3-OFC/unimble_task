using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOFront.Models
{
    public class BillingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? TxID { get; set; }

        [Required(ErrorMessage = "Informe seu nome completo.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Informe o CPF do pagador.")]
        public string? CPF { get; set; }

        public float? Value { get; set; }

        public DateTime? Deadline { get; set; }

        public int UserID { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }
    }
}
