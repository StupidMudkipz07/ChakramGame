using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer render;

    [SerializeField] PlayerController player;
    //jag vill inte behöva coupla spelaren med andra objekt, kan man skicka med sådan data i unityevent?

    private void FlipSprite(bool direction) 
    {
        render.flipX = direction;
    }

    void Start()
    {
        player.ContiniousWalk.AddListener(StartWalk);
        player.ContiniousIdle.AddListener(StartIdle);
        player.ContiniousLookUp.AddListener(StartLookUp);
        player.OnJump.AddListener(OnPlayerJump);
        player.ContiniousCrouch.AddListener(OnPlayerCrouch);
        player.OnDash.AddListener(OnDash);
        player.OnEndDash.AddListener(OnEndDash);

        player.OnLeavingWall.AddListener(OnWallLeave);

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


    void OnWallLeave()
    {
        animator.SetBool("wallSlide", false);
    }

    void Update()
    {
        //transform.

        //sprite flipping
        if(player.velocity.x < 0)
        {
            FlipSprite(true);
        }
        else
        {
             FlipSprite(false);
        }

        animator.SetInteger("yspeed", (int)MathF.Round(player.velocity.y));

        if (player.state == PlayerState.walled)
        {
            animator.SetBool("wallSlide", true);
        }
        else
        {
            animator.SetBool("wallSlide", false);
        }


    }
}