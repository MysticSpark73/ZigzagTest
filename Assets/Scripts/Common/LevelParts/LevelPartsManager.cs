using System.Collections.Generic;
using UnityEngine;
using Zagzag.Common.LevelParts;
using Zagzag.Common.UI.Dialogs;
using Zagzag.Common.UI.Dialogs.MainDialog;
using Zagzag.Core.Data;
using Zagzag.Core.Events;
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

        private void Awake() 
        {
            EventsManager.OnGameRestart += OnGameRestart;
        }

        private void Start()
        {
            queuedParts = new Queue<LevelPart>();
            recyclingParts = new List<LevelPart>();
            PrepareGameStart();
        }

        private void OnApplicationQuit()
        {
            EventsManager.OnGameRestart -= OnGameRestart;
        }

        private void Update()
        {
            if (Parameters.GetGameState() == GameState.Palying)
            {
                CheckReturnPart();
            }
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

        private async void PrepareGameStart() 
        {
            startPart.gameObject.SetActive(true);
            while (queuedParts.Count > 0)
            {
                LevelPart part = queuedParts.Dequeue();
                await part.Hide(false);
            }
            SpawnPart(startPart.GetEndPosition());
            for (int i = 0; i < 5; i++)
            {
                SpawnPart(lastEnqueuedPart.GetEndPosition());
            }
            Parameters.ResetDataOnGameRestart();
            DialogManager.Instance.ShowDialog<MainDialog>();
            EventsManager.OnGamePrepeared?.Invoke();
            Parameters.SetGameState(GameState.Ready);
        }

        private void CheckReturnPart()
        {
            playerPos = Parameters.GetCharacterPos();
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
                Parameters.IncreaseMoveSpeed();
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

        private void OnGameRestart() 
        {
            PrepareGameStart();
            Parameters.SetGameState(GameState.Ready);
        }

    }
}
