using System.Collections.Generic;
using UnityEngine;

namespace GMTK2024.Foundation
{
    public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                T instance = Object.Instantiate(_prefab, _parent);
                instance.gameObject.SetActive(false);
                _pool.Push(instance);
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                T instance = _pool.Pop();
                instance.gameObject.SetActive(true);
                return instance;
            }
            else
            {
                T instance = Object.Instantiate(_prefab, _parent);
                return instance;
            }
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}