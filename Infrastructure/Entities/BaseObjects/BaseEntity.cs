using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Interfaces;

namespace Infrastructure.Entities.BaseObjects;

public class BaseEntity : ICreateableByUser, IUpdateableByUser,IChangeable,IDeleteable,IActiveable
{
    [Required]
    public int? CreatedUserId { get; set; }
    [ForeignKey(nameof(CreatedUserId))]
    public ApplicationUser CreatedUser { get; set; }
    [Required]
    public int? UpdatedUserId { get; set; }
    [ForeignKey(nameof(UpdatedUserId))]
    public ApplicationUser UpdatedUser { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime Updated { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsPublish { get; set; }
}