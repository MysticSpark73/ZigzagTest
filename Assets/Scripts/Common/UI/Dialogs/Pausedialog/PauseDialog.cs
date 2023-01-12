using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zagzag.Core.Data;

namespace Zagzag.Common.UI.Dialogs.PauseDialog
{
    public class PauseDialog : BaseDialog
    {
        [SerializeField] private RectTransform rootTransform;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button exitButton;

        private float showAnimDuration = .5f;
        private float hideanimDuration = .25f;
        private float moveUpDist = Screen.height;

        private Vector2 dialogPos;

        public override async Task Hide(bool animate = true, Action callback = null)
        {
            IsShowing = false;
            bool isComplete = false;
            rootTransform.DOAnchorPosY(moveUpDist, animate ? hideanimDuration : 0).SetEase(Ease.OutQuad).
                OnComplete(() => isComplete = true);
            await new WaitUntil(() => isComplete);
            gameObject.SetActive(false);
            DialogReset();
            callback?.Invoke();
            Parameters.SetGameState(GameState.Ready);
        }

        public override async Task Show(bool animate = true, Action callback = null)
        {
            IsShowing = true;
            bool isComplete = false;
            rootTransform.DOAnchorPosY(dialogPos.y, animate ? showAnimDuration : 0).From(Vector2.up * moveUpDist).SetEase(Ease.OutQuad)
                .OnStart(() => gameObject.SetActive(true))
                .OnComplete(() => isComplete = true);
            await new WaitUntil(() => isComplete);
            callback?.Invoke();
        }

        public override async Task Init(bool animate = true, Action callback = null)
        {
            Parameters.SetGameState(GameState.Pause);
            dialogPos = rootTransform.anchoredPosition;
            SetListeners();
            await base.Init(animate, callback);
        }

        private void SetListeners() 
        {
            returnButton.onClick.RemoveAllListeners();
            returnButton.onClick.AddListener(() => DialogManager.Instance.HideDialog<PauseDialog>());
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitApp);
        }

        private void DialogReset() 
        {
            rootTransform.anchoredPosition = dialogPos;
        }

        private void ExitApp() 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();

#endif
        }
    }
}
