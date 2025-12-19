using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This Script contains the logic to spawn pipes at regular intervals with random sizes for a 2D game. 
/// It is also responsible for make the pipes transform its vector2D. Also destroying pipes that move out of the game area.
/// </summary>

public class PipesSpawner : MonoBehaviour
{
    public GameObject T_base_prefab, T_body_prefab, T_cap_prefab; // Prefabs for the Top pipe direction components
    public GameObject B_base_prefab, B_body_prefab, B_cap_prefab; // Prefabs for the Bottom pipe direction components

    

    Collider2D gameArea; // Collider Variable for the game area
    // GameObject topPipe; // Variable for the top pipe
    // GameObject bottomPipe; // Variable for the bottom pipe

    [Header("Pipes Spawn Settings")]
    [SerializeField] int minSizeLength = 2; // Minimum size of the pipe
    [SerializeField] int maxSizeLength = 3; // Maximum size of the pipe
    [SerializeField] float pipesMoveSpeed = 1f; // Speed at which pipes move left
    [SerializeField] float spawnCooldown = 3f; // Time interval between pipe spawns
    float spawnTimeStart = 1f;

    // get and set pipesmovespeed from pipesmovement script
    public float PipesMoveSpeed
    {
        get { return pipesMoveSpeed; }
        set { pipesMoveSpeed = value; }
    }

    void Start()
    {
        getGameArea();

    }
    void Update()
    {
        spawning();
        destroyingPipes();
    }

    void FixedUpdate()
    {
        movingPipes();
    }

    void spawning()
    {
        spawnTimeStart -= Time.deltaTime;
        if (spawnTimeStart <= 0f)
        {
            float verticalOffset = Random.Range(-4f, 4f);
            Vector2 spawnPosition = new Vector2(15, verticalOffset);
            SpawnPipe(spawnPosition);
            spawnTimeStart = spawnCooldown;

        }
    }

    void getGameArea()
    {
        gameArea = GameObject.Find("GameArea").GetComponent<Collider2D>();
    }


    public void SpawnPipe(Vector2 spawnPosition)
    {
        int topLength = Random.Range(minSizeLength, maxSizeLength + 1);
        int bottomLength = Random.Range(minSizeLength, maxSizeLength + 1);

        // Create parent GameObjects
        GameObject topPipe = new GameObject("TopPipe");
        GameObject bottomPipe = new GameObject("BottomPipe");

        // Set tags
        topPipe.tag = "TopPipe";
        bottomPipe.tag = "BottomPipe";

        // Top Pipe Generation
        Vector2 topStart = spawnPosition + new Vector2(0, topLength + 1);
        Instantiate(T_base_prefab, topStart, Quaternion.identity, topPipe.transform);
        for (int i = 2; i < topLength; i++)
        {
            Vector2 bodyPos = spawnPosition + new Vector2(0, i + 1);
            Instantiate(T_body_prefab, bodyPos, Quaternion.identity, topPipe.transform);
        }
        Instantiate(T_cap_prefab, spawnPosition + new Vector2(0, 2), Quaternion.identity, topPipe.transform);

        // Bottom Pipe Generation
        Vector2 bottomStart = spawnPosition - new Vector2(0, bottomLength + 1);
        Instantiate(B_base_prefab, bottomStart, Quaternion.identity, bottomPipe.transform);
        for (int i = 2; i < bottomLength; i++)
        {
            Vector2 bodyPos = spawnPosition - new Vector2(0, i + 1);
            Instantiate(B_body_prefab, bodyPos, Quaternion.identity, bottomPipe.transform);
        }
        Instantiate(B_cap_prefab, spawnPosition - new Vector2(0, 2), Quaternion.identity, bottomPipe.transform);

        // Score Zone
        GameObject scoreZone = new GameObject("ScoreZone");
        scoreZone.transform.position = spawnPosition;
        scoreZone.transform.parent = topPipe.transform; // Parent to topPipe so it moves with it
        scoreZone.tag = "Score"; // Ensure "Score" tag is defined in Unity Editor
        BoxCollider2D scoreCollider = scoreZone.AddComponent<BoxCollider2D>();
        scoreCollider.size = new Vector2(0.5f, 4f); // Thin collider spanning the gap
        scoreCollider.isTrigger = true;
    }

    void destroyingPipes()
    {
        GameObject[] topPipes = GameObject.FindGameObjectsWithTag("TopPipe");
        GameObject[] bottomPipes = GameObject.FindGameObjectsWithTag("BottomPipe");
        foreach (GameObject topPipe in topPipes)
        {
            foreach (Transform child in topPipe.transform)
            {
                if (child.name == "ScoreZone") continue; // Skip score collider for bounds check
                Collider2D topPipeCollider = child.GetComponent<Collider2D>();
                if (!gameArea.bounds.Contains(topPipeCollider.bounds.min) &&
                    !gameArea.bounds.Contains(topPipeCollider.bounds.max))
                {
                    Destroy(topPipe);
                }
            }
        }
        foreach (GameObject bottomPipe in bottomPipes)
        {
            foreach (Transform child in bottomPipe.transform)
            {
                Collider2D bottomPipeCollider = child.GetComponent<Collider2D>();
                if (!gameArea.bounds.Contains(bottomPipeCollider.bounds.min) &&
                    !gameArea.bounds.Contains(bottomPipeCollider.bounds.max))
                {
                    // Destroy(bottomPipeCollider.transform.gameObject);
                    Destroy(bottomPipe);
                }
            }
        }
    }

    void movingPipes()
    {
        GameObject[] topPipes = GameObject.FindGameObjectsWithTag("TopPipe");
        GameObject[] bottomPipes = GameObject.FindGameObjectsWithTag("BottomPipe");
        foreach (GameObject topPipe in topPipes)
        {
            topPipe.transform.Translate(Vector2.left * pipesMoveSpeed * Time.deltaTime);
        }
        foreach (GameObject bottomPipe in bottomPipes)
        {
            bottomPipe.transform.Translate(Vector2.left * pipesMoveSpeed * Time.deltaTime);
        }
    }
}
