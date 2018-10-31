namespace Bld.WinVkSdk.Cache.InMemory
{
    using System;
    using AutoMapper;

    internal class CacheObject<T> where T : class 
    {
        public CacheObject(T value)
        {
            Value = value;
            UpdateTime = DateTime.Now;
        }

        public T Value { get; }

        public DateTime UpdateTime { get; private set; }

        public static implicit operator T(CacheObject<T> obj)
        {
            return obj?.Value;
        }

        public void Update(T newValue)
        {
            UpdateTime = DateTime.Now;
            Mapper.Map(newValue, Value);
        }
    }
}
