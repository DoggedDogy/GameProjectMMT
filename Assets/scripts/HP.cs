using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject destroyed;
    
    [Header("Visual components")]
    [SerializeField] Image healthBarI;
    [SerializeField] Image healthBarII;
    [SerializeField] Text healthText;
    [SerializeField] Image DeathScreen1;
    [SerializeField] Image DeathScreen2;


    Animator anim;
    int StartHealth;

    void Start()
    {
        StartHealth = health;
        anim = GetComponent<Animator>();
    }

    public void GetDamage(int Damage)
    {
        if (anim)
        {
            anim.SetInteger("state", (int)States.damage);
        }

        health -= Damage;

        if (healthText) healthText.text = health.ToString();
        if (healthBarI)
        {
            healthBarI.fillAmount = ((1f * health) / (1f * StartHealth));
            healthBarII.fillAmount = ((1f * health) / (1f * StartHealth));
        }

        if (health <= 0)
        {
            if (GetComponent<skeleton>())
            {
                anim.SetInteger("state", (int)States.death);                
                foreach (Collider E in GetComponents<Collider>())
                {
                    Destroy(E);
                }
                Destroy(this);
                Destroy(GetComponent<skeleton>());
            }
            else
            {
                if (destroyed)
                {
                    GameObject.Instantiate(
                        destroyed,
                        transform.position,
                        transform.rotation,
                        transform.parent
                        );
                    Destroy(gameObject);
                }
                else
                {
                    if (DeathScreen1)
                    {
                        DeathScreen1.fillAmount = 1;
                        DeathScreen2.fillAmount = 1;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
