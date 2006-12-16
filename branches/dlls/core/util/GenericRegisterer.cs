using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    public interface IIDable
    {
        string ID{
            get;
        }
    }

    public interface IRegisterer
    {
        bool BaseRegister(object o);
        bool Unregister(string name);
    }

    public class GenericRegisterer<TType> : IRegisterer
        where TType : IIDable
    {
        private Dictionary<string, TType> registeredTypes = new Dictionary<string, TType>();

        /// <summary>
        /// Gets the registered type under the given name
        /// </summary>
        /// <param name="id">the id of the type</param>
        /// <returns>The TType, if found, else throws a KeyNotFoundException</returns>
        public TType this[string id]
        {
            get { return registeredTypes[id]; }
        }

        /// <summary>
        /// Registers the given type
        /// </summary>
        /// <param name="registerable">The type to register</param>
        /// <returns>true if successful or already registered; false otherwise</returns>
        public virtual bool Register(TType registerable)
        {
            if (registeredTypes.ContainsKey(registerable.ID))
            {
                if (registerable.Equals(registeredTypes[registerable.ID]))
                {
                    return true;
                }
                return false;
            }
            registeredTypes.Add(registerable.ID, registerable);
            return true;
        }

        /// <summary>
        /// Unregisters the type under the given name.
        /// </summary>
        /// <param name="name">name of thing to unregister</param>
        /// <returns></returns>
        public virtual bool Unregister(string name)
        {
            if (registeredTypes.ContainsKey(name))
                registeredTypes.Remove(name);
            return true;
        }

        public ICollection<TType> Values
        {
            get { return registeredTypes.Values; }
        }

        public TType[] ValuesArray
        {
            get
            {
                lock (this)
                {
                    TType[] array = new TType[Values.Count];
                    Values.CopyTo(array, 0);
                    return array;
                }
            }
        }

        #region IRegisterer Members
        public bool BaseRegister(object o)
        {
            return Register((TType)o);
        }
        #endregion
    }
}
