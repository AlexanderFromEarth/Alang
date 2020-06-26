using System;
using System.Collections.Generic;
using System.Linq;

namespace Lang.Interpreting.Types
{
  class Function : ICallable, IPrintable
  {
    public Func<IReadOnlyList<object>, object> Body { get; }
    public Function(Func<IReadOnlyList<object>, object> body) => Body = body;
    public object Call(IReadOnlyList<object> args) => Body.Invoke(args);
    public virtual string GetPrintString() => "lambda-function";
  }
}