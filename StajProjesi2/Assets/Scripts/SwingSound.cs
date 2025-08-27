using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSound : MonoBehaviour
{
    Vector3 startRotation;
    Vector3 currentRotation;
    AudioSource audioSource;
    List<AudioClip> audioClips = new List<AudioClip>();
    float playDuration = 3f;
    float timer = 0f;
    bool playOnSound;
    int n;
    public FootStepCollection[] soundSwing;
    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        currentRotation = transform.rotation.eulerAngles;
        Vector3 diff  = currentRotation - startRotation;
        if (playOnSound)
        {
            
            if (diff.x >1) 
            {
                timer += Time.deltaTime;
                if (timer <=playDuration && !audioSource.isPlaying)
                {
                    n = Random.Range(0, audioClips.Count);
                    audioSource.clip = audioClips[n];
                    audioSource.PlayOneShot(audioSource.clip, 0.3f);
                    timer = 0f;
                }
                timer = 0f;
            }
            if(diff.x > 0  && diff.x < 1)
            {
                playOnSound = false;
                timer = 0f;
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        AddSoundSwing();
        if(collision.gameObject.CompareTag("Player"))
        {
            playOnSound = true;
        }
    }
    void AddSoundSwing()
    {
        foreach (FootStepCollection sound in soundSwing)
        {
            audioClips.Clear();
            for (int i = 0; i < sound.footStepSounds.Count; i++)
            {
                audioClips.Add(sound.footStepSounds[i]);
            }
        }
    }
}
