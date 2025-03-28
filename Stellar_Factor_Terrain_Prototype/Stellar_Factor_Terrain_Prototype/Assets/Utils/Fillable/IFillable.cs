public interface IFillable<T> where T : IFiller
{
    void FillWith(T item);
    T Empty();
}
