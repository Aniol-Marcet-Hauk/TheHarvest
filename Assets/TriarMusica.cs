using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriarMusica : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip music1, music2, music3;
    [SerializeField] private float minWait = 15f, maxWait = 150f,speed = .5f;

    void Start()
    {
        musicSource.enabled = false;
        StartCoroutine(m());
    }
    private IEnumerator m()
    {
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        int rand = Random.Range(0, 2);
        
        StartCoroutine(playSong(music2, 182));
        
    }
    IEnumerator playSong(AudioClip song, float timeSong)
    {
        
        musicSource.clip = song;
        musicSource.enabled = true;
        float t = 0;
        while (t < .4f)
        {
            musicSource.volume = t;
            t += Time.fixedDeltaTime* speed;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(timeSong);
        musicSource.volume = 0;
        musicSource.clip = null;
        musicSource.enabled = false;
        StartCoroutine(m());
    }
}
