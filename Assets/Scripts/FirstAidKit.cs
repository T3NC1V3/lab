using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    private Collectable heart;

    private void Start()
    {
        heart = new Collectable("heart", 0, 5);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Player")
        {
            heart.UpdateHealth();
            Destroy(gameObject);
        }
    }


}
