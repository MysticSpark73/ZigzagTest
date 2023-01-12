using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zagzag.Core.Data;

namespace Zagzag.Common.UI.Dialogs.LevelDialog
{
    public class LevelDialog : BaseDialog
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button pauseButton;

        public override async Task Hide(bool animate = true, Action callback = null)
        {
            IsShowing = false;
            gameObject.SetActive(false);
            callback?.Invoke();
            await new WaitForUpdate();
        }

        public override async Task Show(bool animate = true, Action callback = null)
        {
            IsShowing = true;
            gameObject.SetActive(true);
            callback?.Invoke();
            await new WaitForUpdate();
        }

        public override async Task Init(bool animate = true, Action callback = null)
        {
            SetListeners();
            await base.Init(animate, callback);
        }

        public void UpdateScore(int score) 
        {
            scoreText.text = score.ToString();
        }

        private void SetListeners() 
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(() => DialogManager.Instance.ShowDialog<PauseDialog.PauseDialog>());
        }
    }
}
