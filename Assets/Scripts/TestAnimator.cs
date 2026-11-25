using System;
using UnityEngine;

public class TestAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    [SerializeField] PlayerController player;
    //jag vill inte behöva coupla spelaren med andra objekt, kan man skicka med sådan data i unityevent?


    public void Idle()
    {
        animator.SetTrigger("Idle");
    }

    public void Moving()
    {
        animator.SetTrigger("Run");
    }

    public void StartDash(float a)
    {
        //animator.SetBool("Dash", true);
        animator.SetTrigger("Dash");
    }

    public void EndDash()
    {
        //animator.SetBool("Dash", false);
    }

    public void StartFall()
    {
        //animator.SetBool("Dash", true);
        animator.SetTrigger("Falling");
    }

    public void StartRising()
    {
        //animator.SetBool("Dash", true);
        animator.SetTrigger("Rising");
    }

    public void StartWallSliding()
    {
        //animator.SetBool("Dash", true);
        animator.SetTrigger("Wall");
    }




    void Update()
    {

    }
}
