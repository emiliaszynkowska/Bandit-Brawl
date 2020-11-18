using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSound : MonoBehaviour
{

    public SoundManager sound;
    // Start is called before the first frame update
    void Start()
    {
        if (sound == null) sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        StartCoroutine(StartMusic());
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(10);
        sound.PlayMusic();
        yield return null;
    }
}
