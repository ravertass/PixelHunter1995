
using System;

namespace PixelHunter1995.Utilities
{
    public static class Utils
    {

        /// <summary>
        /// Throws an exception (with given message) if object is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T NotNull<T>(T obj, string msg, params string[] args)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format(msg, args));
            }
            return obj;
            //x NotNull(obj, typeof(ArgumentNullException), msg, args);
        }
        public static T NotNull<T>(T obj, Type exception, string msg, params string[] args)
        {
            if (obj == null)
            {
                throw (Exception)Activator.CreateInstance(exception, string.Format(msg, args));
            }
            return obj;
        }

    }
}
