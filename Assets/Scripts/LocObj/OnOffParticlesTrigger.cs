using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffParticlesTrigger : MonoBehaviour
{
    public bool disactivateTrigger;
    public GameObject[] particleSystems;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CharacterController2D player))
        {
            if(disactivateTrigger)
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (particleSystems[i].activeInHierarchy)
                    {
                        particleSystems[i].SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < particleSystems.Length; i++)
                {
                    if (!particleSystems[i].activeInHierarchy)
                    {
                        particleSystems[i].SetActive(true);
                    }
                }
            }
        }
    }
}
