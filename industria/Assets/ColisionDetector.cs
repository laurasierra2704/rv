using UnityEngine;

public class ColisionDetector : MonoBehaviour
{
    private GameController _gameController;

    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Ignora el piso
        if (hit.normal.y > 0.5f) return;

        Debug.Log($"Colision con: {hit.gameObject.name}");
        _gameController?.VibrarMotor();
    }
}