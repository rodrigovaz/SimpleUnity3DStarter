using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    public Vector3 walkPoint;
    private bool isWalkPointSet;
    public float walkPointRange;
    public NavMeshAgent agent;

    public GameObject keyPrefab;

    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isWalkPointSet = false;
    }

    private void patrolling() {

        //make surer that the patrol only happens when there is a point to go to
        if(!isWalkPointSet) searchWalkPoint();
        else agent.SetDestination(walkPoint);

        Vector3 distance = transform.position - walkPoint;

        //stop when it's already close enough
        if(distance.magnitude < 4.0f)
            isWalkPointSet = false;
    }

    private void searchWalkPoint() {
        RaycastHit hit;
        Vector3 pos;
        NavMeshPath navMeshPath = new NavMeshPath();
        float zpos = 0.0f, xpos = 0.0f;

        zpos = transform.position.z - Random.Range(-walkPointRange, walkPointRange);
        xpos = transform.position.x - Random.Range(-walkPointRange, walkPointRange);

        pos = new Vector3(xpos, 1000.0f, zpos);

        if(zpos < 0) zpos = 0;
        if(xpos < 0) xpos = 0;

        if(xpos > 1000) xpos = 1000;
        if(zpos > 1000) zpos = 1000;

        //search for a point on the terrain that is reachable
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity) &&
        (agent.CalculatePath(hit.point, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete))
        {
            if(hit.collider.gameObject.tag == "terrain") {
                walkPoint = new Vector3(xpos, hit.point.y, zpos);
                isWalkPointSet = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        patrolling();
    }
}
