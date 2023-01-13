using DG.Tweening;
using UnityEngine;
using Zagzag.Common.Audio;
using Zagzag.Core.Data;

namespace Zagzag.Common.Gems
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private GameObject particleEffect;
        [SerializeField][Range(1, 10)] private int gemsValue;

        private float idleAnimDuration = 1.0f;
        private float hideAnimDuration = .5f;
        private float idleAnimValue = 1f;

        Vector3 initialScale;

        public void Init() 
        {
            initialScale = transform.localScale;
            gameObject.SetActive(true);
            IdleAnim();
        }

        private void IdleAnim() 
        {
            transform.DOMoveY(idleAnimValue, idleAnimDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void HideAnim() 
        {
            transform.DOScale(Vector3.zero, hideAnimDuration).SetEase(Ease.OutSine).OnStart(() => {
                particleEffect.SetActive(true);
            }).OnComplete(() => {
                gameObject.SetActive(false);
                transform.localScale = initialScale;
                particleEffect.SetActive(false);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            Parameters.AddGems(1);
            AudioController.Instance.PlaySound(Sounds.GemPickup);
            HideAnim();
        }


    }
}
