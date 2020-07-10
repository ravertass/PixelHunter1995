

using System;

namespace PixelHunter1995.Components
{
    interface IComponent
    {
    }

    /// <summary>
    /// Extension methods for stuff implementing hte interface
    /// </summary>
    static class Utils
    {
        // TODO consider making a NullDependencyException exception
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

            //if (obj == null)
            //{
            //    throw new ArgumentNullException(String.Format("{0}: A dependency ({1}) was given a null value",
            //        _this.GetType().Name, name));
            //}
            //return obj;
            return NotNull(obj, "{0}: A dependency ({1}) was given a null value", _this.GetType().Name, name);
        }
        /// <summary>
        /// Throws an exception (with given message) if object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T NotNull<T>(T obj, String msg, params String[] args)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(String.Format(msg, args));
            }
            return obj;
        }
    }
}
