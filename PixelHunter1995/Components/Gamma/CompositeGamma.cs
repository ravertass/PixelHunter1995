using System;
using System.Collections.Generic;

namespace PixelHunter1995.Components.Gamma
{
    class CompositeGamma
    {

        /// <summary>
        /// Using type as key, to ensire only one instance exists.
        /// Could have used HashSet instead, but this allows for retrieving the instance easier.
        /// Could also have used a string with classname instead.
        /// </summary>
        Dictionary<Type, IComponentGamma> components = new Dictionary<Type, IComponentGamma>();

        public CompositeGamma()
        {

        }

        public void Init()
        {
            AssertDependencies();

            foreach (IComponentGamma component in components.Values)
            {
                component.Init(this);
            }
        }

        /// <summary>
        /// With this strategy, we can't varify dependencies statically at compile-time.
        /// Instead, this method is called before initializing the components, throwing an exception at failure.
        /// </summary>
        /// <returns></returns>
        public bool AssertDependencies()
        {
            foreach (IComponentGamma component in components.Values)
            {
                foreach (Type dependency in component.Dependencies ?? System.Linq.Enumerable.Empty<Type>())
                {
                    if (!this.HasComponent(dependency))
                    {
                        throw new Exception("SpriteComponentGamma: Lacking dependencies! - " + dependency + " not found.");
#pragma warning disable CS0162 // Unreachable code detected
                        return false; // if we disable the exception.
#pragma warning restore CS0162 // Unreachable code detected
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Avoid using, no guarantee Type implements IComponentGamma.
        /// Try using method<T>() instead.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public IComponentGamma GetComponent(Type component)
        {
            IComponentGamma result;
            components.TryGetValue(component, out result);
            return result;
        }

        public T GetComponent<T>()
            where T : IComponentGamma
        {
            IComponentGamma result;
            components.TryGetValue(typeof(T), out result);
            return (T) result;
        }

        /// <summary>
        /// Avoid using, no guarantee Type implements IComponentGamma.
        /// Try using method<T>() instead.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public IComponentGamma AddComponent(Type component)
        {
            IComponentGamma inst = (IComponentGamma) Activator.CreateInstance(component);
            components.Add(component.GetType(), inst);
            return inst;
        }
        public T AddComponent<T>()
            where T : IComponentGamma
        {
            T inst = Activator.CreateInstance<T>();
            components.Add(typeof(T), inst);
            return inst;
        }
        public CompositeGamma AddComponent(IComponentGamma component)
        {
            components.Add(component.GetType(), component);
            
            // Unlike the others, this can be chained
            return this;
        }

        /// <summary>
        /// Avoid using, no guarantee Type implements IComponentGamma.
        /// Try using method<T>() instead.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public bool HasComponent(Type component)
        {
            return components.ContainsKey(component);
        }
        public bool HasComponent<T>()
            where T : IComponentGamma
        {
            return components.ContainsKey(typeof(T));
        }

    }
}
