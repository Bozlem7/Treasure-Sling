using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //playyer�n kendine has olan class� burda bool de�erleri 
    //ve sa�l�k t�r�nden de�erleri blunaacak
    public Player myPlayer = new Player();//turn managerda atama yapt�k �d atamas�//
    public PlayerMovement cageControls;
    void Start()
    {
        cageControls = GetComponent<PlayerMovement>();
    }

    void Update()
    {
    }

}
