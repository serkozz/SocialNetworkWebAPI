using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models;

[Table(nameof(Subscription))]
public class Subscription
{
#pragma warning disable CS8618
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    [Required]
    public int Id { get; set; }

    public int SubscribingId { get; set; }

    [Column(nameof(SubscribingId))]
    [Required]
    [ForeignKey(nameof(SubscribingId))]
    public virtual Profile SubscribingProfile { get; set; }

    public int SubscribedId { get; set; }

    [Column(nameof(SubscribedId))]
    [Required]
    [ForeignKey(nameof(SubscribedId))]
    public virtual Profile SubscribedProfile { get; set; }

    public Subscription() { }
#pragma warning restore CS8618
}