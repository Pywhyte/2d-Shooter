using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    //what to chaise
    public Transform target;
    //How many times each second we will update our path
    public float updateLate = 2f;
    //caching
    private Seeker seeker;
    private Rigidbody2D rb;
    //calculate path
    public Path path;
    //AI speed per second
    public float speed = 300f;
    public ForceMode2D fmode;
    [HideInInspector]
    public bool pathIsEnded = false;
    //THe max distamce from AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3f;
    //the way point we are currently moving
    private int currentWaypoint = 0;
    private bool searchingForPlayer = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if (target == null)
        {
            if(!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            
            return;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }
    IEnumerator SearchForPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchingForPlayer = false;
            target = searchResult.transform;
            StartCoroutine(UpdatePath());
           yield return false;
        }
       
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }

            yield return false ;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f/ updateLate) ;
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. did it have error" + p.error);
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }

            return;
        }
        if (path == null)
        {
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count)
        {
            if(pathIsEnded)
            {
                return;
            }
            Debug.LogError("Kind of path reached");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        //Move the AI
        rb.AddForce(dir, fmode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}
