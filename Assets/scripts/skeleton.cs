using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States
{
    idle = 0,
    run,
    stay,
    damage,
    death,
    knockback,
    attack,
    skill,
    walk,
}

public class skeleton : MonoBehaviour
{
    Animator anim;
    PlayerController pc;
    [SerializeField] float speed;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (anim.GetInteger("state") >= (int)States.attack
            && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            return;

        if (pc)
        {
            transform.LookAt(pc.transform);
            if (Vector3.Magnitude(pc.transform.position - transform.position) < 4f)
                anim.SetInteger("state", (int)States.attack);
            else
            {
                anim.SetInteger("state", (int)States.run);
                transform.Translate(speed*Vector3.forward);
            }
        }
        else
            anim.SetInteger("state", (int)States.idle);

        //anim.SetInteger("state", (int)States.death);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            pc = other.GetComponent<PlayerController>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            pc = null;
    }

    public void Attack()
    {
        pc.GetComponent<HP>().GetDamage(50);
    }
}
