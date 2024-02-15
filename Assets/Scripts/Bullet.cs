using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRb;
    PlayerMovement playerMovement;
    float xSpeed;
    [SerializeField] float bulletSpeed;
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
        bulletRb.velocity = new Vector2 (xSpeed, 0);

        Destroy(this.gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(other.gameObject);            
        }
        Destroy(this.gameObject);
    }
}
