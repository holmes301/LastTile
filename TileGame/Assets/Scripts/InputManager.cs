using UnityEngine;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputMap playerInput;
    [SerializeField] private UIInputMap uiInput;
    private bool _playerActiveMap = true;
    void OnEnable() {
        UIInputMap.OnPauseGame += changeMap;
        UIManager.OnSelectResume += changeMap;

        TileMovement.OnGameOver += changeMap;
    }
    void OnDisable() {
        UIInputMap.OnPauseGame -= changeMap;
        UIManager.OnSelectResume -= changeMap;

        TileMovement.OnGameOver -= changeMap;
    }
    void Start() {
        playerInput.active = true;
        uiInput.active = false;
        _playerActiveMap = true;
    }
    private void changeMap() {
        if (playerInput is not null && uiInput is not null) {
            _playerActiveMap = !_playerActiveMap;
            playerInput.active = _playerActiveMap;
            uiInput.active = !_playerActiveMap;
        }
    }
}
