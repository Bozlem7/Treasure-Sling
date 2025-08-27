using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FootStepSwaper : MonoBehaviour
{
    public FootStepCollection[] FootStepCollections;
    [SerializeField ] FootStepPlayer FootStepPlayer;
    TerrainChecker checker;
    public string currentLayer;
    public LayerMask include;
    void Start()
    {
        checker = new TerrainChecker();

    }
    private void Update()
    {
        CheckLayer();
    }
    public void CheckLayer()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 15, include))
        {
            if(hit.transform.GetComponent<Terrain>() != null)
            {
                Terrain t = hit.transform.GetComponent <Terrain>();
                if (currentLayer != checker.GetLayerName(transform.position, t))
                {
                    currentLayer = checker.GetLayerName (transform.position, t);//terrainin  layer ad�na atanm�� olur 

                    //player�n pozisyonu katman� currentlayera e�itlenmi� olur
                    //foreach i�inde dizi olcak foreachle tekrar edecek 
                    //ama buna de�i�tirdi�miz sesi nas�l at�ycaz bilmiyorum
                    //if(currentLayer == colllection.name)
                    //{
                    //ayak izlerini de�i�tirip ileri geri yapmas�n� sa�l�ycaz 
                    //}
                    foreach (FootStepCollection collection in FootStepCollections)
                    {
                        if(currentLayer == collection.name)
                        {
                            //fpcnin Swapfootstep yapmas�n� isteyecekmi� (collection)
                            FootStepPlayer.SwapFootSteps(collection);

                        }
                    }

                }
            }

        }
    }
}
