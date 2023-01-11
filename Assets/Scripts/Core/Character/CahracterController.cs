using UnityEngine;
using Zagzag.Core.Data;
using Zagzag.Core.Events;

namespace Zagzag.Common.Character
{
    public class CahracterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private float speed;
        private MoveDirection direction = MoveDirection.Left;

        private void Awake()
        {
            OnGetControl();
        }

        private void Update()
        {
            if (transform.position.y < 0)
            {
                OnLoseControl();
                Parameters.SetGameState(GameState.Over);
            }
            if (Parameters.GetGameState() == GameState.Palying)
            {
                Move();
            }
        }

        private void OnApplicationQuit()
        {
            OnLoseControl();
        }

        private void ToggleDirection() 
        {
            switch (direction)
            {
                case MoveDirection.Right:
                    direction = MoveDirection.Left;
                    break;
                case MoveDirection.Left:
                    direction = MoveDirection.Right;
                    break;
            }
        }

        private void Move() 
        {
            switch (direction)
            {
                case MoveDirection.Right:
                    rb.velocity = new Vector3(speed, rb.velocity.y, 0);
                    break;
                case MoveDirection.Left:
                    rb.velocity = new Vector3(0, rb.velocity.y, speed);
                    break;
                default:
                    rb.velocity = Vector3.zero;
                    break;
            }
            Parameters.SetCharacterPos(transform.position);
        }

        private void OnGetControl() 
        {
            EventsManager.OnTap += OnTap;
            EventsManager.OnSpeedChanged += OnSpeedChanged;
            speed = Parameters.GetMoveSpeed();
        }

        private void OnLoseControl() 
        {
            EventsManager.OnTap -= OnTap;
            EventsManager.OnSpeedChanged -= OnSpeedChanged;
        }

        private void OnTap() 
        {
            if (Parameters.GetGameState() == GameState.Palying)
            {
                Parameters.AddScore(1);
                ToggleDirection();
            }
            else if (Parameters.GetGameState() == GameState.Ready)
            {
                Parameters.AddScore(1);
                ToggleDirection();
                Parameters.SetGameState(GameState.Palying);
            }
            
        }

        private void OnSpeedChanged(float newSpeed) 
        {
            
        }

        private enum MoveDirection : byte 
        {
            Right,
            Left
        }

    }
}
