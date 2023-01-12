using System;
using Zagzag.Core.Data;

namespace Zagzag.Core.Events
{
    public static class EventsManager
    {
        #region Input

        public static Action OnTap;

        #endregion

        #region GameState

        public static Action<GameState> OnGameStateChanged;

        public static Action OnGameRestart;

        public static Action OnGamePrepeared;

        #endregion

        #region Character

        public static Action<float> OnSpeedChanged;

        #endregion

        #region Score

        public static Action<int> OnScoreChanged;

        #endregion
    }
}
