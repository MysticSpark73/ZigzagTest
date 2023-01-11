using UnityEngine;
using Zagzag.Core.Data;
using Zagzag.Core.Events;

namespace Zagzag.Core.Character
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Vector3 offset = new Vector3(-5, 5, -5);

        private Vector3 velocity;
        private Transform target;
        private float smoothTime = .25f;
        private bool isFollowing;

        private void Awake()
        {
            target = gameObject.transform;
            EventsManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void LateUpdate()
        {
            if (!isFollowing)
            {
                return;
            }
            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, target.position + offset, ref velocity, smoothTime);
        }

        private void OnApplicationQuit()
        {
            EventsManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameReady()
        {
            isFollowing = true;
        }

        private void OnGameOver()
        {
            isFollowing = false;
        }

        private void OnGameStateChanged(GameState state) 
        {
            if (state == GameState.Ready)
            {
                OnGameReady();
            }
            if (state == GameState.Over)
            {
                OnGameOver();
            }
        }
    }
}
