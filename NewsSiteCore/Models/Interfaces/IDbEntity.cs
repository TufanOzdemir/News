namespace Models.Interfaces
{
    public interface IDbEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
