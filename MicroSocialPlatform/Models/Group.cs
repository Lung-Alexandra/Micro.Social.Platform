using System.ComponentModel.DataAnnotations;


namespace MicroSocialPlatform.Models;

public class Group
{

    [Key]
    public int GroupId { get; set; }
    [Required(ErrorMessage = "Numele grupului este obligatoriu!")]
    [MinLength(4, ErrorMessage = "Numele unui grup trebuie sa fie mai lung de 4 caractere!")]
    [MaxLength(60, ErrorMessage = "Numele unui grup nu poate sa contina mai mult de 60 de caractere!")]
    public string GroupName { get; set; }
    [StringLength(400, ErrorMessage = "Descriere nu poate sa fie mai lunga de 400 de caractere")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
    public GroupStatusFlag Status { get; set; }
    public virtual ICollection<GroupMember> GroupMembers { get; set; }

    public enum GroupStatusFlag
    {
        MessageGroup,
        PrivateConversation
    };
}