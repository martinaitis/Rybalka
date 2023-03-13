using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rybalka.Core.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string PasswordHash { get; set; }

        public List<FishingNote> FishingNotes { get; set; }
    }
}
