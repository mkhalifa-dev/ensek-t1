namespace Lib.Interfaces
{
    public interface IValidator<T>
    {
        Task<bool> IsValidAsync(T model);
    }
}
