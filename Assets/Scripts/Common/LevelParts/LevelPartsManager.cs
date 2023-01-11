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
        private List<LevelPart> recyclingParts;

        Vector3 playerPos;

        private void Start()
        {
            queuedParts = new Queue<LevelPart>();
            recyclingParts = new List<LevelPart>();
            PrepareGameStart();
        }

        private void Update()
        {
            CheckReturnPart();
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
            for (int i = 0; i < 5; i++)
            {
                SpawnPart(lastEnqueuedPart.GetEndPosition());
            }
            Parameters.SetGameState(GameState.Ready);
        }

        private async void CheckReturnPart()
        {
            playerPos = Parameters.GetCharacterPos();
            //Debug.Log($"Player pos = {playerPos}");
            //Debug.Log($"First part delta = {Vector3.Distance(queuedParts.Peek().GetEndPosition(), queuedParts.Peek().transform.position)}");
            if (startPart.gameObject.activeSelf)
            {
                if (Vector3.Distance(playerPos, startPart.transform.position) > Vector3.Distance(startPart.GetEndPosition(), startPart.transform.position))
                {
                    RecyclePart(startPart);
                    return;
                }
            }
            if (Vector3.Distance(playerPos, queuedParts.Peek().transform.position) > Vector3.Distance(queuedParts.Peek().GetEndPosition(), queuedParts.Peek().transform.position))
            {
                RecyclePart(queuedParts.Dequeue());
            }
        }

        private async void RecyclePart(LevelPart part) 
        {
            if (recyclingParts.Contains(part))
            {
                return;
            }
            recyclingParts.Add(part);
            await part.Hide();
            SpawnPart(lastEnqueuedPart.GetEndPosition());
            recyclingParts.Remove(part);
        }

    }
}
