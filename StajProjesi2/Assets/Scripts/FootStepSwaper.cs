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
                    currentLayer = checker.GetLayerName (transform.position, t);//terrainin  layer adýna atanmýþ olur 

                    //playerýn pozisyonu katmaný currentlayera eþitlenmiþ olur
                    //foreach içinde dizi olcak foreachle tekrar edecek 
                    //ama buna deðiþtirdiðmiz sesi nasýl atýycaz bilmiyorum
                    //if(currentLayer == colllection.name)
                    //{
                    //ayak izlerini deðiþtirip ileri geri yapmasýný saðlýycaz 
                    //}
                    foreach (FootStepCollection collection in FootStepCollections)
                    {
                        if(currentLayer == collection.name)
                        {
                            //fpcnin Swapfootstep yapmasýný isteyecekmiþ (collection)
                            FootStepPlayer.SwapFootSteps(collection);

                        }
                    }

                }
            }

        }
    }
}
