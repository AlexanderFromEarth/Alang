using System.Collections.Generic;

namespace Lang.Interpreting
{
  public interface ICallable
  {
    Dictionary<string, object> Closure { get; }
    object Call(IReadOnlyList<object> args);
  }
}