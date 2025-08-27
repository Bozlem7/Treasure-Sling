using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //playyerýn kendine has olan classý burda bool deðerleri 
    //ve saðlýk türünden deðerleri blunaacak
    public Player myPlayer = new Player();//turn managerda atama yaptýk ýd atamasý//
    public PlayerMovement cageControls;
    void Start()
    {
        cageControls = GetComponent<PlayerMovement>();
    }

    void Update()
    {
    }

}
