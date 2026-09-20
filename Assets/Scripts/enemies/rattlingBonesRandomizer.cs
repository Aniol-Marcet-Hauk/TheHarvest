using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rattlingBonesRandomizer : MonoBehaviour
{

    public Enemy enScript;
    private AudioSource _as;
    void Start()
    {
        _as = transform.GetComponent<AudioSource>();
        _as.enabled = false;
        StartCoroutine(soundON());
        
    }
    private IEnumerator soundON()
    {
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        _as.enabled = true;
        while(!enScript.Death)
        {
            _as.volume = Random.Range(0, .6f);
            yield return new WaitForSeconds(Random.Range(.5f, 1f));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
