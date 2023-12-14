using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Entities;

namespace Infrastructure.Interfaces;

public interface IUpdateableByUser
{
    int? UpdatedUserId { get; set; }
    [ForeignKey(nameof(UpdatedUserId))]
    ApplicationUser UpdatedUser { get; set; }
}