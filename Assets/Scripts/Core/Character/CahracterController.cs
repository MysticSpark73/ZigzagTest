using System;
using UnityEngine;
using Zagzag.Core.Data;
using Zagzag.Core.Events;

namespace Zagzag.Common.Character
{
    public class CahracterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private Vector3 startPos = Vector3.up * .25f;
        private float speed;
        private MoveDirection direction = MoveDirection.Left;

        private void Awake()
        {
            EventsManager.OnGameRestart += OnGameRestart;
            OnGetControl();

        }

        private void Update()
        {
            if (transform.position.y < startPos.y - .1f)
            {
                OnLoseControl();
                Parameters.SetGameState(GameState.Over);
                Parameters.UpdateHighScore();
            }
            if (Parameters.GetGameState() == GameState.Palying)
            {
                Move();
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }

        private void OnApplicationQuit()
        {
            EventsManager.OnGameRestart -= OnGameRestart;
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

        private void OnGameRestart()
        {
            transform.position = Vector3.up *5f;
            direction = MoveDirection.Left;
            OnGetControl();
        }

        private enum MoveDirection : byte 
        {
            Right,
            Left
        }

    }
}
