namespace Repository.Interfaces.Actions
{
    public interface IUpdateRepository<T, y> where T : class
    {
        string Update(T item, y id);
    }
}
