using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zagzag.Core.Events;

namespace Zagzag.Common.UI.Dialogs.GameOverDialog
{
    public class GameOverDialog : BaseDialog
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI highScore;
        [SerializeField] private RectTransform bodyTransform;
        [SerializeField] private RectTransform retryButtonTransform;
        [SerializeField] private Button retryButton;

        private float showAnimDuration = .5f;
        private float hideAnimDuration = .25f;

        private float rightMoveDist = Screen.width;

        private Vector2 titlePos;
        private Vector2 bodyPos;
        private Vector2 retryPos;

        public override async Task Hide(bool animate = true, Action callback = null)
        {
            IsShowing = false;
            bool isComplete = false;
            DOTween.Kill(this);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, retryButtonTransform.DOAnchorPosX(rightMoveDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? hideAnimDuration : 0, bodyTransform.DOAnchorPosX(rightMoveDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? hideAnimDuration *2 : 0, title.rectTransform.DOAnchorPosX(rightMoveDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.OnComplete(() => isComplete = true);
            sequence.Play();
            await new WaitUntil(() => isComplete);
            gameObject.SetActive(false);
            DialogReset();
            callback?.Invoke();
        }

        public override async Task Show(bool animate = true, Action callback = null)
        {
            IsShowing = true;
            bool isComplete = false;
            DOTween.Kill(this);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, title.rectTransform.DOAnchorPosX(titlePos.x, animate ? showAnimDuration : 0).From(Vector2.right * rightMoveDist).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? showAnimDuration : 0, bodyTransform.DOAnchorPosX(bodyPos.x, animate ? showAnimDuration : 0).From(Vector2.right * rightMoveDist).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? showAnimDuration * 2 : 0, retryButtonTransform.DOAnchorPosX(retryPos.x, animate ? showAnimDuration : 0).From(Vector2.right * rightMoveDist).SetEase(Ease.OutQuad));
            sequence.OnStart(() => gameObject.SetActive(true));
            sequence.OnComplete(() => isComplete = true);
            sequence.Play();
            await new WaitUntil(() => isComplete);
            callback?.Invoke();
        }

        public override async Task Init(bool animate = true, Action callback = null)
        {
            titlePos = title.rectTransform.anchoredPosition;
            bodyPos = bodyTransform.anchoredPosition;
            retryPos = retryButtonTransform.anchoredPosition;
            SetListeners();
            await base.Init(animate, callback);
        }

        private void SetListeners() 
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => EventsManager.OnGameRestart?.Invoke());
        }

        private void DialogReset() 
        {
            title.rectTransform.anchoredPosition = titlePos;
            bodyTransform.anchoredPosition = bodyPos;
            retryButtonTransform.anchoredPosition = retryPos;
        }
    }
}
