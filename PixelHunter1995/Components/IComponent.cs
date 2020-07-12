

using System;

namespace PixelHunter1995.Components
{
    interface IComponent
    {
    }

    /// <summary>
    /// Extension methods for stuff implementing the interface
    /// </summary>
    static class Utils_IComponent
    {
        /// <summary>
        /// Throws an exception (with null-dependency message) if object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_this"></param>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T NotNullDependency<T>(this IComponent _this, T obj, String name)
        {
            return Utils.NotNull(obj, typeof(NullDependencyException), "{0}: A dependency ({1}) was given a null value", _this.GetType().Name, name);
        }
    }

    public class NullDependencyException : Exception
    {
        public NullDependencyException()
        {
        }

        public NullDependencyException(string message)
            : base(message)
        {
        }

        public NullDependencyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
