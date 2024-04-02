using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public enum CharacterStance
    {
        Normal,
        Defensive,
        Offensive
    }

    public enum MovementStates
    {
        None,
        Idle,
        Walk,
        Run,
        Attack
    }

    public class AnimationController : MonoBehaviour
    {
        public static AnimationController Instance;

        private MovementStates _currentState;
        public CharacterStance characterStance
        {
            get => characterStance;
            set
            {
                //Debug.Log($"Character Stance Set to: {value}");
                switch (value)
                {
                    case CharacterStance.Normal:
                        _animatorController.SetBool("isNormal", true);
                        _animatorController.SetBool("isDefensive", false);
                        _animatorController.SetBool("isOffensive", false);
                        break;
                    case CharacterStance.Defensive:
                        _animatorController.SetBool("isNormal", false);
                        _animatorController.SetBool("isDefensive", true);
                        _animatorController.SetBool("isOffensive", false);
                        break;
                    case CharacterStance.Offensive:
                        _animatorController.SetBool("isNormal", false);
                        _animatorController.SetBool("isDefensive", false);
                        _animatorController.SetBool("isOffensive", true);
                        break;
                }
            }
        }
        public MovementStates CurrentState
        {
            get => _currentState;
            set
            {
                if (_animatorController == null)
                {
                    _currentState = value;
                    return;
                }

                //Debug.Log($"Character MovementState Set To: {value}");
                switch (value)
                {

                    case MovementStates.None:
                        _animatorController.SetBool("isIdle", false);
                        _animatorController.SetBool("isWalk", false);
                        _animatorController.SetBool("isAttack", false);
                        break;
                    case MovementStates.Idle:
                        _animatorController.SetBool("isIdle", true);
                        _animatorController.SetBool("isWalk", false);
                        _animatorController.SetBool("isAttack", false);
                        break;
                    case MovementStates.Walk:
                        _animatorController.SetBool("isIdle", false);
                        _animatorController.SetBool("isWalk", true);
                        _animatorController.SetBool("isAttack", false);
                        break;
                    case MovementStates.Attack:
                        _animatorController.SetBool("isIdle", false);
                        _animatorController.SetBool("isWalk", false);
                        _animatorController.SetBool("isAttack", true);
                        break;
                }
                _currentState = value;
            }
        }

        public Animator _animatorController;
        public SpriteRenderer _spriteRenderer;

        private Vector3 initialScale;
        private void Awake()
        {
            initialScale = _spriteRenderer.gameObject.transform.localScale;
        }

        public void FlipSprite(bool flip)
        {
            if (flip)
            {
                _spriteRenderer.gameObject.transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            }
            else
            {
                _spriteRenderer.gameObject.transform.localScale = initialScale;
            }
            //_spriteRenderer.flipX = flip;
        }

        public void SetCharacterStance(CharacterStance stance)
        {
            characterStance = stance;
        }

        public void ChangeCharacterAnimator(GameObject charPrefab)
        {
            var tempAnim = _animatorController.gameObject;
            var charClone = Instantiate(charPrefab, _spriteRenderer.transform);

            _animatorController = charClone.GetComponent<Animator>();
            Destroy(tempAnim);
        }
    }
}
