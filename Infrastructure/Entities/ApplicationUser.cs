using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class ApplicationUser: IdentityUser<int>, ICreateableByUser, IUpdateableByUser, IChangeable, IActiveable, IDeleteable
{
    
    public int? UpdatedUserId { get; set; }
    [ForeignKey(nameof(UpdatedUserId))]
    public ApplicationUser UpdatedUser { get; set; }
    [Required]
    public int? CreatedUserId { get; set; }
    [ForeignKey(nameof(CreatedUserId))]
    public ApplicationUser CreatedUser { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool IsPublish { get; set; }
    public bool IsDeleted { get; set; }
}