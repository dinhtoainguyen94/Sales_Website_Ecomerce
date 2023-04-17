namespace Repository.Interfaces.Actions
{
    public interface IUpdateRepository<T> where T : class
    {
        string Update(T item);
    }
}
