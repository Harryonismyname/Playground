using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    public GameObject ball;
    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void CreateBall()
    {
        source.Play();
        ball = Instantiate(ballPrefab);
    }
    public void DestroyBall()
    {
        Destroy(ball);
    }
    public void ResetBall()
    {
        DestroyBall();
        CreateBall();
    }
    public int BallCheck()
    {
        if (ball.transform.position.x > 16)
        {
            return 1;
        }
        if (ball.transform.position.x < -16)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
}
