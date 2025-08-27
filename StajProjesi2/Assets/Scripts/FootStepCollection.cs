using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Foodstep Collections", menuName = "Create New Foodstep Collections")]
public class FootStepCollection : ScriptableObject
{
    public List<AudioClip> footStepSounds = new List<AudioClip>();
}
