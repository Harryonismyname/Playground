using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{

    /// <summary>
    /// Used to determine if two Vector3's are equal
    /// </summary>
    /// <param name="destination">Object's desired location</param><param name="location">Object's Current location</param>
    /// <returns>bool</returns>
    public static bool PositionCheck(Vector3 location, Vector3 destination)
    {
        if (location == destination)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CreateEmptyMeshArrays(int quadCount, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles)
    {
        vertices = new Vector3[4 * quadCount];
        uvs = new Vector2[4 * quadCount];
        triangles = new int[6 * quadCount];
    }

    public static void AddToMeshArrays(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2 uv00, Vector2 uv11)
    {

        int vIndex = index * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex+1;
        int vIndex2 = vIndex+2;
        int vIndex3 = vIndex+3;

        baseSize *= 0.5f;

        bool skewed = baseSize.x != baseSize.y;
        if (skewed)
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x,  baseSize.y);
            vertices[vIndex1] = pos + GetQuaternionEuler(rot) * new Vector3(-baseSize.x, -baseSize.y);
            vertices[vIndex2] = pos + GetQuaternionEuler(rot) * new Vector3(  baseSize.x, baseSize.y);
            vertices[vIndex3] = pos + GetQuaternionEuler(rot) * baseSize;
        }
        else
        {
            vertices[vIndex0] = pos + GetQuaternionEuler(rot-270) * baseSize;
            vertices[vIndex1] = pos + GetQuaternionEuler(rot-180) * baseSize;
            vertices[vIndex2] = pos + GetQuaternionEuler(rot- 90) * baseSize;
            vertices[vIndex3] = pos + GetQuaternionEuler(rot-  0) * baseSize;
        }

        uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
        uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
        uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
        uvs[vIndex3] = new Vector2(uv11.x, uv11.y);

        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;




    }

    private static Quaternion[] cachedQaternionEulerArr;
    private static void CacheQuaternionEuler()
    {
        if (cachedQaternionEulerArr != null) return;
        cachedQaternionEulerArr = new Quaternion[360];
        for (int i = 0; i < 360; i++)
        {
            cachedQaternionEulerArr[i] = Quaternion.Euler(0, 0, i);
        }
    }
    public static Quaternion GetQuaternionEuler(float rotFloat)
    {
        int rot = Mathf.RoundToInt(rotFloat);
        rot = rot % 360;
        if (rot < 0) rot += 360;
        if (cachedQaternionEulerArr == null) CacheQuaternionEuler();
        return cachedQaternionEulerArr[rot];
    }

    public static T RandomEnum<T>()
    {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(1, values.Length - 1)];
    }

    public static bool NullCheck<T>(T subject)
    {
        if (subject != null)
        {
            return true;
        }
        else return false;
    }

    public static Vector3 MouseToWorldPosition()
    {
        Camera cam = GameObject.FindObjectOfType<Camera>();
        Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
        return position;
    }

    public static Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    public static void MoveObjectToLocation(GameObject gameObject, Vector3 destination, float moveSpeed)
    {
        Transform location = gameObject.transform;
        Debug.DrawLine(location.position, destination);
        location.position = Vector3.MoveTowards(location.position, destination, moveSpeed * Time.deltaTime);
    }
    public static void MoveObjectInDirection(GameObject gameObject, Vector3 direction, float moveSpeed)
    {
        Transform transform = gameObject.transform;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
    
    public static bool ObjectInBounds(GameObject gameObject, float xBound, float yBound)
    {
        Transform transform = gameObject.transform;
        if (transform.position.x > -xBound && transform.position.x < xBound && transform.position.y > -yBound && transform.position.y < yBound)
        {
            return true;
        }
        else return false;
    }

    public static void RestrainObjectInBounds(GameObject gameObject, float xBound, float yBound)
    {
        Transform transform = gameObject.transform;
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound-0.1f, transform.position.y, transform.position.z);
        }
        if(transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound+0.1f, transform.position.y, transform.position.z);
        }
        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound-0.1f, transform.position.z);
        }
        if(transform.position.y < -yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound+0.1f, transform.position.z);
        }
    }

}
