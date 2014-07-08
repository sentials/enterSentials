namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://justmikesmith.blogspot.com/2012/06/utilizing-command-pattern-to-support.html
    public interface ICommand : IExecutableObject
    { }

    public interface ICommand<TParameters> : ICommand
    {

    }
}
