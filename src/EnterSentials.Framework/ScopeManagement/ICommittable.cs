namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/decoupling-your-application-domain-from.html
    public interface ICommittable
    {
        void Commit();
    }
}