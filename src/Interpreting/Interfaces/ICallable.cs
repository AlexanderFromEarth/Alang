using System.Collections.Generic;

namespace Lang.Interpreting
{
  public interface ICallable
  {
    object Call(IReadOnlyList<object> args);
  }
}