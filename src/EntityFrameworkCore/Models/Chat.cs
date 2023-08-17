using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models;

[Table(nameof(Chat))]
public class Chat
{
#pragma warning disable CS8618
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    [Required]
    public int Id { get; set; }

    [Column(nameof(Name))]
    [Required]
    public string Name { get; set; }

    [Column(nameof(CreationTime))]
    [Required]
    public virtual DateTime CreationTime { get; set; }

    [Column(nameof(Participants))]
    [Required]
    public virtual List<Profile> Participants { get; set; }

    public Chat() { }
#pragma warning restore CS8618
}