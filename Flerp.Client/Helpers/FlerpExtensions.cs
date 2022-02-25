using System;
using System.Windows.Forms;

namespace Flerp.Client.Helpers
{
    public static class FlerpExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired) target.Invoke(action);
            else action();
        }

        internal static T ThrowIfNull<T>(this T argument, string paramName = null) where T : class
        {
            if (argument != null) return argument;
            if (paramName == null) throw new ArgumentNullException();
            throw new ArgumentNullException(paramName);
        }
    }
}
