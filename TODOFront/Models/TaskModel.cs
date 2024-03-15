using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOFront.Models
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Informe uma tarefa.")]
        public string? Description { get; set; }

        public int Status { get; set; } = 0;

        public int OrganizerId { get; set; }

        [JsonIgnore]
        public UserModel? Organizer { get; set; }
    }
}
