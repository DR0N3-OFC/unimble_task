using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TODOBack.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Informe um nome de usuário.")]
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public bool IsPremium { get; set; } = false;

        [JsonIgnore]
        public List<TaskModel>? Tasks { get; set; } = new List<TaskModel>();

        [JsonIgnore]
        public BillingModel? Billing { get; set; }
    }
}
