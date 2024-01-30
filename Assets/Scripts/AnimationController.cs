using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts
{
    public enum MovementStates
    {
        None,
        Walk,
        Run,
        Attack
    }

    public class AnimationController : MonoBehaviour
    {
        public static AnimationController Instance;

        private MovementStates _currentState;
        public MovementStates CurrentState
        {
            get => _currentState;
            set
            {
                switch (value)
                {
                    case MovementStates.None:
                        _animatorController.SetBool("Walk", false);
                        _animatorController.SetBool("Run", false);
                        break;
                    case MovementStates.Walk:
                        _animatorController.SetBool("Walk", true);
                        _animatorController.SetBool("Run", false);
                        break;
                    case MovementStates.Run:
                        _animatorController.SetBool("Run", true);
                        _animatorController.SetBool("Walk", false);
                        break;
                    case MovementStates.Attack:
                        _animatorController.SetTrigger("Attack");
                        _animatorController.SetBool("Walk", false);
                        _animatorController.SetBool("Run", false);
                        break;
                }

                _currentState = value;
            }
        }

        public Animator _animatorController;
        public SpriteRenderer _spriteRenderer;

        public void FlipSprite(bool flip)
        {
            _spriteRenderer.flipX = flip;
        }
    }
}
