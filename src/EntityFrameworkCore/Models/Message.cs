using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models;

[Table(nameof(Message))]
public class Message 
{
#pragma warning disable CS8618
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    [Required]
    public int Id { get; set; }

    public int ChatId { get; set; }

    [Column(nameof(ChatId))]
    [Required]
    [ForeignKey(nameof(ChatId))]
    public Chat Chat { get; set; }

    public int ProfileId { get; set; }

    [Column(nameof(ProfileId))]
    [Required]
    [ForeignKey(nameof(ProfileId))]
    public virtual Profile Profile { get; set; }

    [Column(nameof(CreationTime))]
    [Required]
    public virtual DateTime CreationTime { get; set; }

    [Column(nameof(Edited))]
    [Required]
    public bool Edited { get; set; }

    public int RepliedMessageId { get; set; }

    [Column(nameof(RepliedMessageId))]
    public Message RepliedMessage { get; set; }

    public Message() { }
#pragma warning restore CS8618
}