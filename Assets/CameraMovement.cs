using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameRunner runner;
    private GridMap<MapNode> map;
    private float xBound;
    private float yBound;
    private readonly float moveSpeed = 15f;

    void Update()
    {
        if (map == null)
        {
            if (Tools.NullCheck(runner.map))
            {
                map = runner.map.GetGrid();
                xBound = map.Width;
                yBound = map.Height;
            }
        }
        Move();
    }

    private void Move()
    {
        Tools.RestrainObjectInBounds(gameObject, xBound, yBound);
        if (Tools.ObjectInBounds(gameObject, xBound, yBound))
        {
            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                Tools.MoveObjectInDirection(gameObject, new Vector3(Input.GetAxis("Horizontal"), 0, 0), moveSpeed);
            }
            if (Input.GetAxisRaw("Vertical") != 0f)
            {
                Tools.MoveObjectInDirection(gameObject, new Vector3(0, Input.GetAxis("Vertical"), 0), moveSpeed);
            }
        }

    }
}
