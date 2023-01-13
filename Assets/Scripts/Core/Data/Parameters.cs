using UnityEngine;
using Zagzag.Core.Events;

namespace Zagzag.Core.Data
{
    public static class Parameters
    {
        #region ObjectPoolerKeys

        //should be the same as respective prefabs names
        public static readonly string pooler_key_part_zigzag = "PartZigzag";
        public static readonly string pooler_key_part_up = "PartUp";
        public static readonly string pooler_key_part_long_up = "PartLongUp";

        public static string GetRandomPoolerKey()
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    return pooler_key_part_zigzag;
                case 1:
                    return pooler_key_part_up;
                case 2:
                    return pooler_key_part_long_up;
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region GameState

        private static GameState gameState = GameState.Loading;

        public static GameState GetGameState() => gameState;

        public static void SetGameState(GameState state) 
        {
            if (gameState != state)
            {
                gameState = state;
                EventsManager.OnGameStateChanged?.Invoke(gameState);
            }
        }

        public static void ResetDataOnGameRestart() 
        {
            ResetScore();
            ResetMoveSpeed();
        }

        #endregion
        #region Character

        private static float moveSpeed = 3;
        private static float moveSpeedIncrement = .1f;
        private static Vector3 characterPos = Vector3.zero;

        public static float GetMoveSpeed() => moveSpeed;

        public static void IncreaseMoveSpeed()
        {
            moveSpeed += moveSpeedIncrement;
            EventsManager.OnSpeedChanged?.Invoke(moveSpeed);
        }

        public static void ResetMoveSpeed()
        {
            moveSpeed = 3;
            EventsManager.OnSpeedChanged?.Invoke(moveSpeed);
        }

        public static Vector3 GetCharacterPos() 
        {
            return characterPos;
        }

        public static void SetCharacterPos(Vector3 pos) 
        {
            characterPos = pos;
        }

        #endregion
        #region Score

        private static int score = 0;
        private static int highScore = 0;
        private static int gems = 0;

        public static void AddScore(int value) 
        {
            score += value;
            EventsManager.OnScoreChanged?.Invoke(score);
        }

        public static int GetScore() => score;

        public static int GetHighScore() => highScore;

        public static void ResetScore() 
        {
            score = 0;
        }

        public static void UpdateHighScore() 
        {
            highScore = score > highScore ? score : highScore;
        }

        public static void AddGems(int value) 
        {
            gems += value;
        }

        public static int GetGems() => gems;

        #endregion

        #region Cheat

        private static bool isCheating = false;

        public static void SetCheating(bool value)
        {
            isCheating = value;
            EventsManager.OnIsCheatingChanged(isCheating);
        }

        public static bool GetIsCheating() => isCheating;

        #endregion
    }
}
