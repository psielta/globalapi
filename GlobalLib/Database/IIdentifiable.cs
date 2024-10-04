namespace GlobalLib.Database
{
    public interface IIdentifiable<TKey>
    {
        TKey GetId();
        string GetKeyName();
    }

}
