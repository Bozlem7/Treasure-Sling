using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CageSpawn : MonoBehaviour
{
    
    [SerializeField] GameObject cageObj;
    GameObject instance;
    PlayerController playerControl;
    public  TurnManager turnManager;
    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        playerControl = GetComponent<PlayerController>();
        turnManager = FindObjectOfType<TurnManager>();
       
    }

    void Update()
    {
        
    }

}
