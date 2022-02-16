using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TreePlanter : MonoBehaviour
{
    public GameObject treePrefab;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        var rand = new System.Random();
        
        //plant 1000 trees
        for(int i = 0; i < 1000; i++) {
            float xpos = 1000.0f*(float)rand.NextDouble();
            float ypos = 1000.0f*(float)rand.NextDouble();

            //this was only added to try and find an interesting pattern
            if(Mathf.PerlinNoise(xpos, ypos) > 0.5f) {
                Vector3 pos = new Vector3(xpos, 1000.0f, ypos);
                Quaternion rot = Quaternion.identity;
                rot.eulerAngles = new Vector3(0.0f, (float)rand.Next(179), 0.0f);

                if (Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    if(hit.point.y > 60.0f && hit.collider.gameObject.name == "Terrain") {
                        var tempTree = Instantiate(treePrefab, hit.point, rot);
                        float scale = 0.3f + (2.0f * (float)rand.NextDouble());
                        tempTree.transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
