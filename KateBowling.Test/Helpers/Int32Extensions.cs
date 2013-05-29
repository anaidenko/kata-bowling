using System;

namespace KataBowling.Test.Helpers
{
    public static class Int32Extensions
    {
        public static void Times(this Int32 times, Action action)
        {
            for (var i = times; i > 0; i--)
            {
                action();
            }
        }
    }
}