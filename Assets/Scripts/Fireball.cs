using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SoundManager sound;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Destroy(gameObject,10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character chara = collision.gameObject.GetComponent<Character>();
        if (chara != null)
        {
            chara.TakeDamage(damage);
            Destroy(gameObject);
            sound.PlayFire();

        }

    }
}
