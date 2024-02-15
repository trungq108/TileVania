using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinSFX;
    [SerializeField] int coinPoint;
    bool isPickUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPickUp)
        {
            isPickUp = true;
            FindObjectOfType<GameSession>().AddScore(coinPoint);
            AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }             
    }
}
