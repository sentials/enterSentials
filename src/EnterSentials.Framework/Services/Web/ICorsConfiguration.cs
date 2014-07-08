namespace EnterSentials.Framework
{
    public interface ICorsConfiguration
    {
        string AllowedOrigin { get; }
        string AllowedMethods { get; }
        string AllowedHeaders { get; }
    }
}