using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

[Table(nameof(Profile))]
public class Profile
{
#pragma warning disable CS8618
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    [Required]
    public int Id { get; set; }

    public int AuthId { get; set; }
    [Column(nameof(AuthId))]
    [Required]
    [ForeignKey(nameof(AuthId))]
    [JsonIgnore]
    public virtual Auth Auth { get; set; }

    [Column(nameof(ProfileName))]
    [Required]
    [StringLength(9)]
    public string ProfileName { get; set; }

    [Column(nameof(FirstName))]
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Column(nameof(LastName))]
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Column(nameof(Nickname))]
    [StringLength(20)]
    public string? Nickname { get; set; }

    public Profile() { }
#pragma warning restore CS8618
}