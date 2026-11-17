using System;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] PlayerController player;
    //jag vill inte behöva coupla spelaren med andra objekt, kan man skicka med sådan data i unityevent?

    bool isDashing = false;



    public void StartDash()
    {
        //animator.SetBool("Dash", true);
    }

    public void EndDash()
    {
        //animator.SetBool("Dash", false);
    }


    void Update()
    {
        animator.SetBool("Dash", player.IsDashing);

        animator.SetFloat("ySpeed", player.velocity.y);
        animator.SetInteger("xSpeed", (int)MathF.Round(Math.Abs(player.velocity.x)));
    }
}
