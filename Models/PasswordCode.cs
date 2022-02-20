using System.ComponentModel.DataAnnotations;

namespace LoginLogout.Models
{
    public class PasswordCode
    {
        public int Id { get; set; }
        public Users User { get; set; }
        public int UserId { get; set; }
        [StringLength(6)]
        public string Code { get; set; }
    }
}
