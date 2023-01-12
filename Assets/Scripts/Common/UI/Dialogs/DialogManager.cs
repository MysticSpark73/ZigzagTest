using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zagzag.Core.Data;
using Zagzag.Core.Events;

namespace Zagzag.Common.UI.Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance;
        [SerializeField] private List<BaseDialog> dialogs;

        private void Awake()
        {
            Instance = this;
            EventsManager.OnGameStateChanged += OnGameStateChanged;
            EventsManager.OnGameRestart += OnGameRestart;
        }

        private void OnApplicationQuit()
        {
            EventsManager.OnGameStateChanged -= OnGameStateChanged;
            EventsManager.OnGameRestart -= OnGameRestart;
        }

        public async void ShowDialog<T>(bool animate = true, Action callback = null)
        {
            var dialog = dialogs.FirstOrDefault(d => d is T);
            if (dialog == null)
            {
                Debug.LogError($"Requested dialog of type {typeof(T)} is not in the dialogs list");
                return;
            }
            if (dialog.IsShowing)
            {
                Debug.LogWarning($"Requested dialog {dialog} is already showing");
                return;
            }
            await dialog.Init(animate, callback);
        }

        public async void HideDialog<T>(bool animate = true, Action callback = null)
        {
            var dialog = dialogs.FirstOrDefault(d => d is T);
            if (dialog == null)
            {
                Debug.LogError($"Requested dialog of type {typeof(T)} is not in the dialogs list");
                return;
            }
            if (!dialog.IsShowing)
            {
                Debug.LogWarning($"Requested dialog {dialog} is already hidden");
            }
            if (dialog.IsShowing)
            {
                await dialog.Hide(animate, callback);
            }
        }

        public bool IsDialogShowing<T>() 
        {
            return dialogs.FirstOrDefault(d => d is T).IsShowing;
        }

        private void OnGameStateChanged(GameState state) 
        {
            switch (state)
            {
                case GameState.Palying:
                    if (IsDialogShowing<MainDialog.MainDialog>())
                    {
                        HideDialog<MainDialog.MainDialog>();
                        ShowDialog<LevelDialog.LevelDialog>(true, () => HideDialog<MainDialog.MainDialog>());
                    }
                    break;
                case GameState.Ready:
                    break;
                case GameState.Over:
                    HideDialog<LevelDialog.LevelDialog>(true, () => ShowDialog<GameOverDialog.GameOverDialog>());
                    break;
                case GameState.Pause:
                    break;
                case GameState.Loading:
                    break;
                default:
                    break;
            }
        }

        private void OnGameRestart() 
        {
            HideAll();
            //ShowDialog<MainDialog.MainDialog>();
        }

        private async void HideAll(bool animate = true) 
        {
            foreach (var dialog in dialogs)
            {
                if (dialog.IsShowing)
                {
                    await dialog.Hide(animate);
                }
            }
        }


    }
}