using System;
using UnityEngine;
using Zagzag.Common.Audio;
using Zagzag.Core.Data;
using Zagzag.Core.Events;

namespace Zagzag.Common.Character
{
    public class CahracterController : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private Vector3 startPos = Vector3.up * .25f;
        private Vector3 lookDirection;
        private float speed;
        private bool isCheating;
        private MoveDirection direction = MoveDirection.Left;

        private int layerMask;

        private void Awake()
        {
            EventsManager.OnGamePrepeared += OnGamePrepeared;
            EventsManager.OnIsCheatingChanged += OnIsCheatingChanged;
            layerMask = LayerMask.GetMask("Cheat");

        }

        private void Update()
        {
            if (transform.position.y < startPos.y - .1f && Parameters.GetGameState() == GameState.Palying)
            {
                OnLoseControl();
                Parameters.UpdateHighScore();
                Parameters.SetGameState(GameState.Over);
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
            EventsManager.OnGamePrepeared -= OnGamePrepeared;
            EventsManager.OnIsCheatingChanged -= OnIsCheatingChanged;
            OnLoseControl();
        }

        private void ToggleDirection() 
        {
            switch (direction)
            {
                case MoveDirection.Right:
                    direction = MoveDirection.Left;
                    lookDirection = Vector3.forward;
                    break;
                case MoveDirection.Left:
                    direction = MoveDirection.Right;
                    lookDirection = Vector3.right;
                    break;
            }
            AudioController.Instance.PlaySound(Sounds.Ball);
        }

        private void Move() 
        {
            if (isCheating)
            {
                Debug.DrawRay(transform.position, lookDirection, Color.red, .25f);
                if (Physics.Raycast(transform.position, lookDirection , .5f, layerMask))
                {
                    ToggleDirection();
                }
            }
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
            EventsManager.OnSpeedChanged += OnSpeedChanged;
            speed = Parameters.GetMoveSpeed();
            isCheating = Parameters.GetIsCheating();
            EventsManager.OnTap += OnTap;
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
                if (!isCheating)
                {
                    ToggleDirection();
                    Parameters.AddScore(1);
                }
            }
            else if (Parameters.GetGameState() == GameState.Ready)
            {
                lookDirection = Vector3.forward;
                Parameters.SetGameState(GameState.Palying);
                if (!isCheating)
                {
                    ToggleDirection();
                    Parameters.AddScore(1);
                }
            }

        }

        private void OnSpeedChanged(float newSpeed) 
        {
            speed = newSpeed;
        }

        private void OnIsCheatingChanged(bool value) 
        {
            isCheating = value;
        }

        private void OnGamePrepeared() 
        {
            transform.position = Vector3.up * 5f;
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
