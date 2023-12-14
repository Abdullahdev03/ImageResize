namespace Core.Interfaces.IModel;

public interface ITreeModel<TModel>
{
    int Id { get; set; }
    public int? ParentId { get; set; }
    List<TModel> Children { get; set; }
}
