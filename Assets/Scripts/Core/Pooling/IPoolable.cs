using UnityEngine;

namespace Zagzag.Core.Pooling
{
    public interface IPoolable
    {
        void OnPool();

        void OnReturn();

        Transform GetTransform();
    }
}
