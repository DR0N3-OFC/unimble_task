
using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TODOBack.Models
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
