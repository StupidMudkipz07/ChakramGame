using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] PlayerController player;
    //jag vill inte behöva coupla spelaren med andra objekt, kan man skicka med sådan data i unityevent?

    Vector2 lastPlayerVelocity = Vector2.zero;

    void Start()
    {
        player.ContiniousWalk.AddListener(StartWalk);
        player.ContiniousIdle.AddListener(StartIdle);
        player.ContiniousLookUp.AddListener(StartLookUp);
        player.OnJump.AddListener(OnPlayerJump);
        player.ContiniousCrouch.AddListener(OnPlayerCrouch);
        player.OnDash.AddListener(OnDash);
        player.OnEndDash.AddListener(OnEndDash);
        player.OnGrabbingWall.AddListener(OnWallSlide);
        player.OnWallJump.AddListener(OnWallJump);
    }

    void StartWalk()
    {
        animator.SetBool("Walk", true);
        animator.SetBool("LookingUp", false);
        animator.SetBool("Crouch", false);
    }

    void StartIdle()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Crouch", false);
        animator.SetBool("LookingUp", false);
    }

    void StartLookUp()
    {
        animator.SetBool("LookingUp", true);
        animator.SetBool("Crouch", false);
    }

    void OnPlayerJump()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Crouch", false);
        animator.SetBool("LookingUp", false);
    }

    void OnPlayerCrouch()
    {
        animator.SetBool("Crouch", true);
        animator.SetBool("LookingUp", false);
    }

    void OnDash()
    {
        animator.SetBool("Dash", true);
    }

    void OnEndDash()
    {
        animator.SetBool("Dash", false);
    }

    void OnWallSlide()
    {
        animator.SetBool("wallSlide", true);
    }

    void OnWallJump()
    {
        animator.SetBool("wallSlide", false);
    }

    void Update()
    {
        animator.SetInteger("yspeed", (int)player.velocity.y);
    }
}