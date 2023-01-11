using UnityEngine;
using Zagzag.Core.Events;

namespace Zagzag.Core.Input
{
    public class InputManager : MonoBehaviour
    {
        public static bool IsInputLocked { get; private set; }

        void Update()
        {
            CheckInput();
        }

        public static void LockInput() => IsInputLocked = true;

        public static void UnlockInput() => IsInputLocked = false;

        private void CheckInput() 
        {
            if (IsInputLocked)
            {
                return;
            }

#if UNITY_EDITOR

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.Input.mousePosition.x < Screen.width / 2)
                {
                    EventsManager.TapLeft?.Invoke();
                }
                if (UnityEngine.Input.mousePosition.x > Screen.width / 2)
                {
                    EventsManager.TapRight?.Invoke();
                }
            }

#elif UNITY_ANDROID

            if (UnityEngine.Input.touchCount > 0)
            {
                if (UnityEngine.Input.GetTouch(0).position.x < Screen.width / 2)
                {
                    EventsManager.TapLeft?.Invoke();
                }
                if (UnityEngine.Input.GetTouch(0).position.x > Screen.width / 2)
                {
                    EventsManager.TapRight?.Invoke();
                }
            }

#endif

        }

    }
}
