namespace ICommand;

public static class ExceptionHandler
{
    public static Action<Exception, ICommand>? Handle { get; set; }
}
