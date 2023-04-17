namespace Repository.Interfaces.Actions
{
    public interface IRemoveRepository<T>
    {
        string Remove(T id);
    }
}
