using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models;

[Table(nameof(Friends))]
public class Friends
{
#pragma warning disable CS8618
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    [Required]
    public int Id { get; set; }

    public int FirstProfileId { get; set; }

    [Column(nameof(FirstProfileId))]
    [Required]
    [ForeignKey(nameof(FirstProfileId))]
    public virtual Profile FirstProfile { get; set; }

    public int SecondProfileId { get; set; }

    [Column(nameof(SecondProfileId))]
    [Required]
    [ForeignKey(nameof(SecondProfileId))]
    public virtual Profile SecondProfile { get; set; }

    public Friends() { }
#pragma warning restore CS8618
}