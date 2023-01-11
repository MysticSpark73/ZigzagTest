using System.Collections.Generic;
using UnityEngine;
using Zagzag.Core.Pooling;

namespace Zagzag.Common.LevelParts
{
    public class LevelPart : MonoBehaviour, IPoolable
    {
        [SerializeField] private List<LevelBlock> blocks;
        [SerializeField] private Transform EndPosition;


        #region InterfaceImplementation

        public void OnPool()
        {
            
        }

        public void OnReturn()
        {
            
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #endregion

        public Vector3 GetEndPosition() => EndPosition.position;

    }
}
