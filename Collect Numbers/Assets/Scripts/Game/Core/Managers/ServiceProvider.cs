using Assets.Scripts.Game.Core.NumberBase;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Game.Core.Managers
{
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, IProvidable> RegisterDictionary = new Dictionary<Type, IProvidable>();

        public static T GetManager<T>() where T : class, IProvidable
        {
            if (RegisterDictionary.ContainsKey(typeof(T)))
            {
                return (T)RegisterDictionary[typeof(T)];
            }

            return null;
        }

        public static T Register<T>(T target) where T : class, IProvidable
        {
            RegisterDictionary.TryAdd(typeof(T), target);
            return target;
        }

        public static NumberFactory GetNumberFactory
        {
            get { return GetManager<NumberFactory>(); }
        }

        public static ParticleManager GetParticleManager
        {
            get { return GetManager<ParticleManager>(); }
        }

        public static ImageLibrary GetImageLibrary
        {
            get { return GetManager<ImageLibrary>(); }
        }
    }
}
