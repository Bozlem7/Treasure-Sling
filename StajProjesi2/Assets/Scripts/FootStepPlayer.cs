using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] FootStepSwaper swapper;
    List<AudioClip> footStepSounds = new List<AudioClip>();
    int n;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    public void PlayFootStepAudio()
    {
        swapper.CheckLayer();
        n = Random.Range(1, footStepSounds.Count - 1);

        //bir sa� bir sol yapmas� laz�m bunun i�in de�i�imi nas�l uyguluycam
        audioSource.clip = footStepSounds[n];
        audioSource.PlayOneShot(audioSource.clip);


        footStepSounds[n] = footStepSounds[0];
        footStepSounds[0] = audioSource.clip;
    }
    public void SwapFootSteps(FootStepCollection collection)
    {
        footStepSounds.Clear();
        //ilk �nce t�m soundslar� clear edece�iz
        for(int i = 0; i < collection.footStepSounds.Count;i++)
        {
            //.Add(collection.Footstep)[�]
            footStepSounds.Add(collection.footStepSounds[i]);
        }
    }
}
