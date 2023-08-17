using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models;

[Table(nameof(Auth))]
public class Auth
{
#pragma warning disable CS8618
    [Column(nameof(Id))]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    [Column(nameof(Email))]
    [Required]
    [StringLength(320)]
    public string Email { get; set; }

    [Column(nameof(Password))]
    [Required]
    [StringLength(120)]
    public string Password { get; set; }

    [Column(nameof(IsAdmin))]
    [Required]
    public bool IsAdmin { get; set; }

    public Auth() { }

    public Auth(string email, string password, bool isAdmin = false) => (Email, Password, IsAdmin) = (email, password, isAdmin);
#pragma warning restore CS8618
}