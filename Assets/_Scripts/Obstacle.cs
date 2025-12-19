using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Collider2D col;
    _GUIscript _GUIscript;

    [System.Obsolete]
    void Start()
    {
        col = GetComponent<Collider2D>();
        _GUIscript = GameObject.FindObjectOfType<_GUIscript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit an obstacle!");
            _GUIscript.gameOver();
            AudioManager.Instance.PlaySFX("PlayerHitObstacle");
        }
    }
}

