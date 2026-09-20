using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriarMusica : MonoBehaviour
{
    [SerializeField] private AudioSource m_MusicSource;
    [SerializeField] private AudioClip m_Music1, m_Music2, m_Music3;
    [SerializeField] private float m_MinWait = 15f, m_MaxWait = 150f, m_Speed = .5f;

    void Start()
    {
        m_MusicSource.enabled = false;
        StartCoroutine(PlayRandomSongWithDelay());
    }

    private IEnumerator PlayRandomSongWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(m_MinWait, m_MaxWait));
        int rand = Random.Range(0, 2);
        
        StartCoroutine(PlaySong(m_Music2, 182));
        
    }

    private IEnumerator PlaySong(AudioClip song, float timeSong)
    {
        
        m_MusicSource.clip = song;
        m_MusicSource.enabled = true;
        float t = 0;
        while (t < .4f)
        {
            m_MusicSource.volume = t;
            t += Time.fixedDeltaTime * m_Speed;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(timeSong);
        m_MusicSource.volume = 0;
        m_MusicSource.clip = null;
        m_MusicSource.enabled = false;
        StartCoroutine(PlayRandomSongWithDelay());
    }
}
