using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSpace
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private float _animSmoothTime = 1f;
        
        private Animator _animator;
        private List<Vector2> _animPositions;
        private Vector2 _targetAnimPosition;
        private Vector2 _currentBlendAnim;
        private Vector2 _animVelocity;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (PlayerMovement.Instance.IsWalking)
            {
                _targetAnimPosition = new Vector2(0f, 2f);
            }
            else
            {
                _targetAnimPosition = PlayerMovement.Instance.MoveInput;
            }
            
            _currentBlendAnim = Vector2.SmoothDamp(_currentBlendAnim, _targetAnimPosition, ref _animVelocity, _animSmoothTime * Time.deltaTime);
            
            _animator.SetFloat("Horizontal", _currentBlendAnim.x);
            _animator.SetFloat("Vertical", _currentBlendAnim.y);
            _animator.SetBool("IsGrounded", PlayerMovement.Instance.IsGrounded);
        }
    }
}

