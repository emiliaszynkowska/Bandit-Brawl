using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour
{

    public GameObject fireball;
    public float range;
    public float maxDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireSpawn());
    }

    IEnumerator FireSpawn()
    {
        yield return new WaitForSeconds(3);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, maxDelay));
            Vector2 position = new Vector2(Random.Range(-range, range) + transform.position.x, transform.position.y);
            Instantiate(fireball, position, Quaternion.identity);
            if (maxDelay >= 0.25f) maxDelay = maxDelay - 0.05f;
        }
    }
}
