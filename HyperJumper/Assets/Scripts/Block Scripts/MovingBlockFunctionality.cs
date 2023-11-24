using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlockFunctionality : BaseBlock
{
    [SerializeField] private Transform waypoint_start;
    [SerializeField] private Transform waypoint_end;
    [SerializeField] private Rigidbody2D platform;
    [Header("Actually important modifiers")]
    [SerializeField] private float movementTime = 3f;
    [SerializeField] private float waypointThreshold = 0.1f; 
    private float distanceToWaypoint;
    private float elapsedTime;
    private float lastFramePosition;
    private bool movingTowardsStart;
    private bool movingTowardsEnd;
    private void Start()
    {
        platform.position = waypoint_start.position;
        movingTowardsEnd = true;
        movingTowardsStart = false;
    }
    private void Update()
    {
        
        if (movingTowardsEnd)
        {
            elapsedTime += Time.deltaTime;
            distanceToWaypoint = Vector2.Distance(platform.position, waypoint_end.position);
            platform.MovePosition(Vector2.Lerp(waypoint_start.position, waypoint_end.position, elapsedTime/movementTime));
        }
        else if (movingTowardsStart)
        {
            elapsedTime += Time.deltaTime;
            distanceToWaypoint = Vector2.Distance(platform.position, waypoint_start.position);
            platform.MovePosition(Vector2.Lerp(waypoint_end.position, waypoint_start.position, elapsedTime/movementTime));
        }
        if (movingTowardsEnd && distanceToWaypoint<waypointThreshold) {
            distanceToWaypoint = Vector2.Distance(platform.position, waypoint_start.position);
            movingTowardsEnd = false;
            movingTowardsStart = true;
            elapsedTime = 0;
        }
        if(movingTowardsStart && distanceToWaypoint<waypointThreshold)
        {
            distanceToWaypoint = Vector2.Distance(platform.position, waypoint_end.position);
            movingTowardsStart = false;
            movingTowardsEnd = true;
            elapsedTime = 0;
        }
    }
}
