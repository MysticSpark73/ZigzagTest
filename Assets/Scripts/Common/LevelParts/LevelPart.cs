using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zagzag.Common.Gems;
using Zagzag.Core.Pooling;

namespace Zagzag.Common.LevelParts
{
    public class LevelPart : MonoBehaviour, IPoolable
    {
        [SerializeField] private List<LevelBlock> blocks;
        [SerializeField] private List<Gem> gems;
        [SerializeField] private Transform EndPosition;
        [SerializeField] private string poolingKey;

        private float blockHideDelay = .25f;
        private float blockHideAnimDuration = 1.5f;
        private float blockHidePos = -10;


        #region InterfaceImplementation

        public void OnPool()
        {
            ReinitGems();
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            ResetBlocks();
            if (string.Equals(poolingKey, string.Empty))
            {
                return;
            }
            ObjectPooler.Instance.ReturnIntoPool(poolingKey, gameObject);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #endregion

        public Vector3 GetEndPosition() => EndPosition.position;

        public async Task Hide(bool animate = true) 
        {
            bool isComplete = false;
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < blocks.Count; i++)
            {
                sequence.Insert(animate ? i * blockHideDelay : 0, blocks[i].transform.DOMoveY(blockHidePos, animate ? blockHideAnimDuration : 0).SetEase(Ease.InQuad));
            }
            sequence.OnComplete(
                () => { isComplete = true; });
            sequence.Play();
            sequence.OnKill(() => OnReturn());
            await new WaitUntil(() => isComplete);
            sequence.Kill();
        }

        private void ResetBlocks() 
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].transform.localPosition = new Vector3(blocks[i].transform.localPosition.x, -0.75f, blocks[i].transform.localPosition.z);
            }
        }

        private void ReinitGems() 
        {
            for (int i = 0; i < gems.Count; i++)
            {
                gems[i].Init();
            }
        }

    }
}
