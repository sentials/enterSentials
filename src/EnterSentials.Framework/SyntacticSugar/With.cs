using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public static class With
    {
        public static IEnumerable<object> Key(params object[] keyValues)
        { return keyValues; }

        public static IEnumerable<object> Parameters(params object[] parameters)
        { return parameters; }

        public static IEnumerable<object> Failures(params object[] failures)
        { return failures; }
    }
}