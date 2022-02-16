using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject keyPrefab;
    public int maxEnemies = 4;
    public int nEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var rand = new System.Random();

        //limit the number of enemies to be spawned
        if(nEnemies < maxEnemies && rand.Next(0, 100) < 2) {
            float xpos = 1000.0f*(float)rand.NextDouble();
            float ypos = 1000.0f*(float)rand.NextDouble();

            Vector3 pos = new Vector3(xpos, 400.0f, ypos);
            Quaternion rot = Quaternion.identity;
            rot.eulerAngles = new Vector3(0.0f, (float)rand.Next(179), 0.0f);

            //cast a ray so that the enemy can be spawned close to the ground
            if (Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {

                //to avoid it being spawned over a roof or bridge or tree
                if(hit.point.y > 60.0f && hit.collider.gameObject.name == "Terrain") {
                    var newEnemy = Instantiate(enemyPrefab, hit.point + new Vector3(0.0f, 4.0f, 0.0f), rot);
                    newEnemy.tag = "enemy";

                    NavMeshPath navMeshPath = new NavMeshPath();
                    NavMeshAgent agent = newEnemy.GetComponent<SimpleEnemyAI>().GetComponent<NavMeshAgent>();
                    newEnemy.GetComponent<SimpleEnemyAI>().keyPrefab = keyPrefab;
                    nEnemies++;

                    //make sure that the enemy can get out of the spawn point
                    if(agent.CalculatePath(new Vector3(0.0f, 0.0f, 0.0f), navMeshPath) && navMeshPath.status != NavMeshPathStatus.PathComplete) {
                        Destroy(newEnemy);
                        nEnemies--;
                    }
                }
            }
        }
    }
}
