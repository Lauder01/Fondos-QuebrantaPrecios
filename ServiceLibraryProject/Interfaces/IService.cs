namespace FQP.Service.Interfaces;

public interface IService<T> 
{
    IEnumerable<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);

}
