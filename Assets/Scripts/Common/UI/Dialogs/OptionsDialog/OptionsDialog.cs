using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Zagzag.Common.UI.Dialogs.OptionsDialog
{
    public class OptionsDialog : BaseDialog
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI soundText;
        [SerializeField] private TextMeshProUGUI cheatText;
        [SerializeField] private Button soundsButton;
        [SerializeField] private Button cheatButton;
        [SerializeField] private Button backButton;
        [SerializeField] private RectTransform bodyTransform;
        [SerializeField] private RectTransform backButtonTransform;
        [SerializeField] private RectTransform blockerTransform;

        private float showAnimDuration = .5f;
        private float hideAnimDuration = .3f;
        private float moveUpDist = Screen.height;

        private Vector2 titlePos;
        private Vector2 bodyTransformPos;
        private Vector2 backButtonPos;

        public override async Task Hide(bool animate = true, Action callback = null)
        {
            IsShowing = false;
            bool isComplete = false;
            DOTween.Kill(this);
            Sequence sequence = DOTween.Sequence();
            sequence.Insert(0, blockerTransform.DOScale(Vector3.zero, animate ? hideAnimDuration * 0.5f : 0).From(Vector3.one).SetEase(Ease.OutQuad));
            sequence.Insert(0, backButtonTransform.DOAnchorPosY(moveUpDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.Insert(hideAnimDuration, bodyTransform.DOAnchorPosY(moveUpDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
            sequence.Insert(hideAnimDuration * 2, title.rectTransform.DOAnchorPosY(moveUpDist, animate ? hideAnimDuration : 0).SetEase(Ease.OutQuad));
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
            sequence.Insert(0, blockerTransform.DOScale(Vector3.one, animate ? showAnimDuration *.5f : 0).From(Vector3.zero).SetEase(Ease.OutQuad));
            sequence.Insert(0, title.rectTransform.DOAnchorPosY(titlePos.y, animate ? showAnimDuration : 0).From(Vector2.up * moveUpDist).SetEase(Ease.OutQuad));
            sequence.Insert(showAnimDuration, bodyTransform.DOAnchorPosY(bodyTransformPos.y, animate ? showAnimDuration : 0).From(Vector2.up * moveUpDist).SetEase(Ease.OutQuad));
            sequence.Insert(showAnimDuration * 2, backButtonTransform.DOAnchorPosY(backButtonPos.y, animate ? showAnimDuration : 0).From(Vector2.up * moveUpDist).SetEase(Ease.OutQuad));
            sequence.OnComplete(() => isComplete = true);
            sequence.OnStart(() => gameObject.SetActive(true));
            sequence.Play();
            await new WaitUntil(() => isComplete);
            callback?.Invoke();
        }

        public override async Task Init(bool animate = true, Action callback = null)
        {
            titlePos = title.rectTransform.anchoredPosition;
            bodyTransformPos = bodyTransform.anchoredPosition;
            backButtonPos = backButtonTransform.anchoredPosition;
            SetListeners();
            await base.Init(animate, callback);
        }

        private void SetListeners() 
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => DialogManager.Instance.HideDialog<OptionsDialog>());
        }

        private void DialogReset() 
        {
            title.rectTransform.anchoredPosition = titlePos;
            bodyTransform.anchoredPosition = bodyTransformPos;
            backButtonTransform.anchoredPosition = backButtonPos;

        }
    }
}
