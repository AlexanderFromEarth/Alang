using System;
using System.Collections.Generic;
using System.Linq;

namespace Lang.Interpreting.Types
{
  class Function : ICallable, IPrintable
  {
    public Dictionary<string, object> Closure { get; set; }
    public Func<IReadOnlyList<object>, object> Body { get; }
    public Function(Func<IReadOnlyList<object>, object> body, Dictionary<string, object> closure = null) => (Body, Closure) = (body, closure);
    public object Call(IReadOnlyList<object> args) => Body.Invoke(args);
    public virtual string GetPrintString() => "lambda-function";
  }
}