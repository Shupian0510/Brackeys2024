using UnityEngine;

namespace GMTK2024.Foundation
{
    public interface IObjectPool<T> where T : MonoBehaviour
    {
        T Get();
        void Release(T obj);
    }
}