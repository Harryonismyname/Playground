using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float yBounds;
    public float movementSpeed = 15;
    private float offset;
    private float ballPos;
    private Rigidbody2D ballBod;
    [SerializeField] GameManager gameManager;
    private BallHandler ballHandler;
    private bool aiStarted;
    // Start is called before the first frame update
    void Start()
    {
        yBounds = GameObject.Find("BG").GetComponent<SpriteRenderer>().size.y / 2 - GetComponent<SpriteRenderer>().size.y / 2;
        aiStarted = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ballHandler = GameObject.Find("BallHandler").GetComponent<BallHandler>();
        StartCoroutine(GenerateOffset(5));
    }

    // Update is called once per frame
    void Update()
    {
        Constrain();
        if (gameManager.gameStart)
        {
            transform.position = new Vector2(transform.position.x, yBounds / 2);
            if (!aiStarted && !gameManager.multiplayer)
            {
                if (gameManager.gameDifficulty == 1)
                {
                    StartEasyAI();
                }
                if (gameManager.gameDifficulty == 2)
                {
                    StartNormalAI();
                }
                if (gameManager.gameDifficulty == 3)
                {
                    StartHardAI();
                }
            }
        }
        if (gameManager.gameEnd && !gameManager.multiplayer)
        {
            if (gameManager.gameDifficulty == 1)
            {
                EndEasyAI();
            }
            if (gameManager.gameDifficulty == 2)
            {
                EndNormalAI();
            }
            if (gameManager.gameDifficulty == 3)
            {
                EndHardAI();
            }
        }
        if (gameManager.gameRunning)
        {
            if (!gameManager.multiplayer)
            {
                if (ballHandler.ball)
                {
                    ballBod = ballHandler.ball.GetComponent<Rigidbody2D>();
                    if (ballBod.velocity.x > 0)
                    {
                        if (gameManager.gameDifficulty == 1)
                        {
                            EasyAI();
                        }
                        else if (gameManager.gameDifficulty == 2)
                        {
                            NormalAI();
                        }
                        else if (gameManager.gameDifficulty == 3)
                        {
                            HardAI();
                        }
                        else if (gameManager.gameDifficulty == 4)
                        {
                            ImpossibleAI();
                        }
                    }
                }
            }
            else
            {
                float vertical = Input.GetAxisRaw("Debug Vertical");
                Vector3 direction = new Vector3(0, vertical, 0);
                if (direction.magnitude > 0.1f)
                {
                    transform.Translate(direction.normalized * movementSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (ballHandler.ball)
            {
                ballBod = ballHandler.ball.GetComponent<Rigidbody2D>();
                ImpossibleAI();
            }
        }
    }
    /// <summary>Checks enemy position and restricts movement beyond Y boundaries</summary>
    private void Constrain()
    {
        if (transform.position.y > yBounds)
        {
            transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
        }
        if (transform.position.y < -yBounds)
        {
            transform.position = new Vector3(transform.position.x, -yBounds, transform.position.z);
        }
    }
    // Easy AI Functions
    private void StartEasyAI()
    {
        StartCoroutine(GenerateOffset(10));
        aiStarted = true;
    }
    /// <summary>Finds ball Yposition and sets enemy Yposition to equal the ball's Yposition plus a randomly generated offset</summary>
    private void EasyAI()
    {
        ballPos = ballHandler.ball.transform.position.y;
        if (transform.position.y < ballPos - offset)
        {
            transform.Translate(new Vector3(0, 1, 0).normalized * movementSpeed * Time.deltaTime);
        }
        if (transform.position.y > ballPos + offset)
        {
            transform.Translate(new Vector3(0, -1, 0) * movementSpeed * Time.deltaTime);
        }
    }
    private void EndEasyAI()
    {
        StopCoroutine(GenerateOffset(10));
    }

    // Normal AI Functions
    private void StartNormalAI()
    {
        StartCoroutine(GenerateOffset(5));
        aiStarted = true;
    }
    /// <summary>Finds ball Yposition and moves enemy to a Yposition equal the Yposition plus a randomly generated offset and doubles movement speed if distance is double the Yposition of the enemy</summary>
    private void NormalAI()
    {
        ballPos = ballHandler.ball.transform.position.y;
        if (transform.position.y < ballPos - offset)
        {
            if (ballPos > transform.position.y * 2)
            {
                transform.Translate(new Vector3(0, 1, 0).normalized * (movementSpeed * 2f) * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(0, 1, 0).normalized * movementSpeed * Time.deltaTime);
            }
        }
        if (transform.position.y > ballPos + offset)
        {
            if (ballPos > transform.position.y * 2)
            {
                transform.Translate(new Vector3(0, -1, 0).normalized * (movementSpeed * 2f) * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(0, -1, 0) * movementSpeed * Time.deltaTime);
            }
        }
    }

    private void EndNormalAI()
    {
        StopCoroutine(GenerateOffset(5));
    }

    // Hard AI Functions
    private void StartHardAI()
    {
        StartCoroutine(GenerateOffset(3));
        aiStarted = true;
    }
    /// <summary>Finds ball Yposition and moves enemy to a Yposition equal the Yposition plus a randomly generated offset and doubles movement speed if distance is double the Yposition of the enemy</summary>
    private void HardAI()
    {
        ballPos = ballHandler.ball.transform.position.y;
        if (transform.position.y < ballPos - offset)
        {
            if (ballPos > transform.position.y * 2)
            {
                transform.Translate(new Vector3(0, 1, 0).normalized * (movementSpeed * 2f) * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(0, 1, 0).normalized * movementSpeed * Time.deltaTime);
            }
        }
        if (transform.position.y > ballPos + offset)
        {
            if (ballPos > transform.position.y * 2)
            {
                transform.Translate(new Vector3(0, -1, 0).normalized * (movementSpeed * 2f) * Time.deltaTime);
            }
            else
            {
                transform.Translate(new Vector3(0, -1, 0) * movementSpeed * Time.deltaTime);
            }
        }
    }

    private void EndHardAI()
    {
        StopCoroutine(GenerateOffset(3));
    }

    // Impossible AI Functions
    /// <summary>Finds ball Yposition and moves enemy to a Yposition equal the Yposition</summary>
    private void ImpossibleAI()
    {
        if (ballBod.velocity.x > 0)
        {
            ballPos = ballHandler.ball.transform.position.y;
            if (transform.position.y < ballPos)
            {
                transform.Translate(new Vector3(0, 1, 0).normalized * movementSpeed * Time.deltaTime);
            }
            if (transform.position.y > ballPos)
            {
                transform.Translate(new Vector3(0, -1, 0) * movementSpeed * Time.deltaTime);
            }
        }
    }

    // Offset Generation
    /// <summary>Continually generates a new offset based on provided range</summary>
    IEnumerator GenerateOffset(int range)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            offset = Random.Range(-range, range);
        }
    }
}
