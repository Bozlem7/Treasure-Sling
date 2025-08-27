using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
    AudioSource audioSource;
    public FootStepCollection[] objectSoundsCollection;
    public List<AudioClip> objectSounds = new List<AudioClip>();
    int n;
    //obje tagleri enumu
    enum ObjectTags
    {
        MetalObject,
        WoodObject,
        SpinningToy,

    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.transform.name);
        AddSounds(collision);
        foreach (String tag in Enum.GetNames(typeof(ObjectTags)))
        {
            
            if (collision.gameObject.tag == tag && tag != "Swing")
            {
                
                n = UnityEngine.Random.Range(0, objectSounds.Count);
               audioSource.clip = objectSounds[n];
                audioSource.PlayOneShot(audioSource.clip);

                objectSounds[n] = objectSounds[0];
                objectSounds[0] = audioSource.clip;
            }
        }


    }
    
    void SwapObjectSounds(FootStepCollection objectCollection)
    {
        objectSounds.Clear();
        for(int i = 0; i < objectCollection.footStepSounds.Count; i++)
        {
            objectSounds.Add(objectCollection.footStepSounds[i]);
        }
    }
    void AddSounds(Collision collision)
    {
        foreach(FootStepCollection sound in objectSoundsCollection)
        {
            if (collision.gameObject.tag == sound.name)
            {
                SwapObjectSounds(sound);
            }
        } 
    }
}
