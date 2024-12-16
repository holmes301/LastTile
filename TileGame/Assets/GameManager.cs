using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static event Action<Vector2> OnLevelUp;
    [SerializeField] private bool multiplayer = false;
    [SerializeField] private float secondsPerLevel = 60.0f;
    private float _currSeconds = 0;

    void OnEnable() {
        if (!multiplayer) {
            TileMovement.OnGameOver += handleGameOver;
        }
        else {
            MultiplayerTileMovement.OnGameOver += handleGameOver;
        }
    }
    void OnDisable() {
        if (!multiplayer) {
            TileMovement.OnGameOver -= handleGameOver;
        }
        else {
            MultiplayerTileMovement.OnGameOver -= handleGameOver;
        }
    }
    void Start() {
        Time.timeScale = 1.0f;
    }
    void Update() {
        _currSeconds += Time.deltaTime;
        if (_currSeconds >= secondsPerLevel) {
            _currSeconds %= secondsPerLevel;
            handleLevelUp();
        }
    }
    private void handleGameOver() {
        Time.timeScale = 0.0f;
    }
    private void handleLevelUp() {
        int chosen = UnityEngine.Random.Range(0, 2);
        if (chosen == 0) { // update speed
            OnLevelUp?.Invoke(new Vector2(0, 0.02f));
        }
        else { // update density
            OnLevelUp?.Invoke(new Vector2(1, 0));
        }
    }
}
