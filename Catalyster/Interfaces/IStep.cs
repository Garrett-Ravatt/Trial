namespace Catalyster.Interfaces
{
    public interface IStep<T>
    {
        public T Step(T subject);
    }
}
