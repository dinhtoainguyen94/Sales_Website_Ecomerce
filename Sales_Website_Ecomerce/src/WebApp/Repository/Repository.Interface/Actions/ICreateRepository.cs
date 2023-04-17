namespace Repository.Interfaces.Actions
{
    public interface ICreateRepository<T> where T : class
    {
        string Create(T item);
    }
}
