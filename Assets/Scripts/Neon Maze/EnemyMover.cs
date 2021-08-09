using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>Custom Behaviour Script to control movement</summary>
public class EnemyMover : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Hunting,
        Running
    }
    private Vector3 roamingPosition;
    private Vector3 currentPosition;
    private State state;
    private EnemyAIPathfinder pathfinder;
    private GameObject player;
    private void Awake()
    {
        currentPosition = transform.position;
        state = State.Roaming;
        roamingPosition = GetRoamingPosition();
        pathfinder = GetComponent<EnemyAIPathfinder>();
        player = GameObject.Find("Player");
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                if (pathfinder.pathEnd)
                {
                    currentPosition = transform.position;
                    roamingPosition = GetRoamingPosition();
                    pathfinder.GeneratePathTo(roamingPosition);
                }
                if (Vector3.Distance(transform.position, player.transform.position) < 5f)
                {
                    pathfinder.FollowPlayer();
                    state = State.Hunting;
                }
                break;
            case State.Hunting:
                if (Vector3.Distance(transform.position, player.transform.position) > 5f)
                {
                    pathfinder.LostPlayer();
                    state = State.Roaming;
                }
                break;

            case State.Running:
                break;
        }

    }
    private Vector3 GetRoamingPosition()
    {
        return currentPosition + Tools.GetRandomDirection() * Random.Range(3, 5);
    }
}
