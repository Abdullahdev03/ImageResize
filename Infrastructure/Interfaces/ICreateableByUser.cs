using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface ICreateableByUser
{
    int? CreatedUserId { get; set; }
    [ForeignKey(nameof(CreatedUserId))]
    ApplicationUser CreatedUser { get; set; }

}