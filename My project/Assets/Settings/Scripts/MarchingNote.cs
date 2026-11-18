using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class MarchingNote : MonoBehaviour
{
    public AudioClip[] sounds;

    public float expectedProgress;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayNote()
    {
        if (sounds.Length > 0 && audioSource != null)
        {
            AudioClip sound = sounds[Random.Range(0, sounds.Length)];

            if (sound != null)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                audioSource.PlayOneShot(sound);
                Destroy(gameObject, 1.5f);
                return;
            }
        }

        Destroy(gameObject);
    }
}
