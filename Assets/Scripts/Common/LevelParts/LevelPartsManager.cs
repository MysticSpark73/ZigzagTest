using System.Collections.Generic;
using UnityEngine;
using Zagzag.Common.LevelParts;
using Zagzag.Core.Data;
using Zagzag.Core.Pooling;

namespace Zagzag
{
    public class LevelPartsManager : MonoBehaviour
    {
        
        [SerializeField] private LevelPart startPart;
        private Queue<LevelPart> queuedParts;
        private LevelPart lastEnqueuedPart;

        private void Start()
        {
            queuedParts = new Queue<LevelPart>();
            PrepareGameStart();
        }

        private void SpawnPart(Vector3 pos) 
        {
            IPoolable pooledObject = ObjectPooler.Instance.SpawnRandom(pos);
            LevelPart part = pooledObject.GetTransform().gameObject.GetComponent<LevelPart>();
            if (part == null)
            {
                Debug.LogError($"The pooled object {pooledObject} does not have a LevelPart component on it");
                return;
            }
            lastEnqueuedPart = part;
            queuedParts.Enqueue(part);
        }

        private void PrepareGameStart() 
        {
            startPart.gameObject.SetActive(true);
            queuedParts.Clear();
            SpawnPart(startPart.GetEndPosition());
            for (int i = 0; i < 15; i++)
            {
                SpawnPart(lastEnqueuedPart.GetEndPosition());
            }
            Parameters.SetGameState(GameState.Ready);
        }
    }
}
