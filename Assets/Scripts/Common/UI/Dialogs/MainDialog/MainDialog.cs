using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Zagzag.Core.Data;

namespace Zagzag.Common.UI.Dialogs.MainDialog
{
    public class MainDialog : BaseDialog
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI tapToStart;
        [SerializeField] private TextMeshProUGUI bestScore;
        [SerializeField] private TextMeshProUGUI gemsCount;
        [SerializeField] private Button optionsButton;
        [SerializeField] private RectTransform optionsButtonTransform;
        [SerializeField] private RectTransform gemsTransform;

        private float upMoveDist = 300;
        private float downMoveDist = 900;
        private float showAnimDuration = 1.0f;
        private float flashDuration = 1.0f;
        private float hideAnimDuration = .5f;

        private Vector2 titlePos;
        private Vector2 bestScorePos;
        private Vector2 optionsButtonPos;
        private Vector2 gemsPos;

        public override async Task Hide(bool animate = true, Action callback = null)
        {
            IsShowing = false;
            bool isComplete = false;
            DOTween.Kill(this);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, title.rectTransform.DOAnchorPosY(upMoveDist, animate ? hideAnimDuration : 0)).SetEase(Ease.OutQuad);
            sequence.Insert(0, bestScore.rectTransform.DOAnchorPosY(-downMoveDist, animate ? hideAnimDuration : 0)).SetEase(Ease.OutQuad);
            sequence.Insert(0, tapToStart.DOFade(0, animate ? hideAnimDuration : 0)).SetEase(Ease.InSine);
            sequence.Insert(animate ? hideAnimDuration *.5f : 0, optionsButtonTransform.DOAnchorPosY(-downMoveDist, animate ? hideAnimDuration : 0)).SetEase(Ease.OutQuad);
            sequence.Insert(animate ? hideAnimDuration : 0, gemsTransform.DOAnchorPosY(-downMoveDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.OnComplete(() => isComplete = true);
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
            sequence.Insert(0, title.rectTransform.DOAnchorPosY(titlePos.y, animate ? showAnimDuration : 0).From(Vector2.up * upMoveDist).SetEase(Ease.OutQuad));
            sequence.Insert(0, bestScore.rectTransform.DOAnchorPosY(bestScorePos.y, animate ? showAnimDuration : 0).From(Vector2.down * downMoveDist).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? showAnimDuration * .5f : 0, optionsButtonTransform.DOAnchorPosY(optionsButtonPos.y, animate ? showAnimDuration : 0).From(Vector2.down * downMoveDist).SetEase(Ease.OutQuad));
            sequence.Insert(animate ? showAnimDuration : 0, gemsTransform.DOAnchorPosY(gemsPos.y, animate ? showAnimDuration *.5f : 0).From(Vector2.down * downMoveDist).SetEase(Ease.OutQuad));
            sequence.OnComplete(() => isComplete = true);
            sequence.OnStart(() => gameObject.SetActive(true));
            sequence.Play();
            await new WaitUntil(() => isComplete);
            tapToStart.DOFade(1, flashDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            callback?.Invoke();
        }

        public override async Task Init(bool animate = true, Action callback = null)
        {
            titlePos = title.rectTransform.anchoredPosition;
            bestScorePos = bestScore.rectTransform.anchoredPosition;
            optionsButtonPos = optionsButtonTransform.anchoredPosition;
            gemsPos = gemsTransform.anchoredPosition;
            SetValues();
            SetListeners();
            await base.Init(animate, callback);
        }

        private void DialogReset()
        {
            title.rectTransform.anchoredPosition = titlePos;
            bestScore.rectTransform.anchoredPosition = bestScorePos;
            optionsButtonTransform.anchoredPosition = optionsButtonPos;
            gemsTransform.anchoredPosition = gemsPos;
        }

        private void SetListeners() 
        {
            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(()=> { DialogManager.Instance.ShowDialog<OptionsDialog.OptionsDialog>(); });
        }

        private void SetValues() 
        {
            bestScore.text = $"Best Score : {Parameters.GetHighScore().ToString()}";
            gemsCount.text = $"X{Parameters.GetGems()}";
        }

    }
}
