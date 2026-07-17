namespace task17;

using System;
using ICommand;

public static class ExceptionHandler
{
    public static Action<Exception, ICommand>? Handle { get; set; }
}
