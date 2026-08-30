using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ZeusInspector.SourceGenerators
{
  public static class Helper
  {
    [Conditional("DEBUG")]
    public static void ThrowIfNull([NotNull] object? value)
    {
      _ = value ?? throw new ArgumentNullException();
    }
  }
}
