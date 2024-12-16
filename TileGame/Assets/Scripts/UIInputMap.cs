using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UIInputMap : MonoBehaviour
{
    public bool active = false;
    public static event Action<Vector2> OnNavigateUI;
    public static event Action OnPauseGame;
    public static event Action OnSelect;
    private InputSystem_Actions _uiInput;
    void Awake() {
        _uiInput = new InputSystem_Actions();
    }
    void OnEnable() {
        _uiInput.UI.Enable();
    }
    void OnDisable() {
        _uiInput.UI.Disable();
    }
    void Update() {
        int pauseGame = (int) _uiInput.UI.TogglePause.ReadValue<float>() * (_uiInput.UI.TogglePause.WasPressedThisFrame() ? 1 : 0);
        if (pauseGame != 0) {
            OnPauseGame?.Invoke();
        }
        if (active) {
            int select = (int) _uiInput.UI.Select.ReadValue<float>() * (_uiInput.UI.Select.WasPressedThisFrame() ? 1 : 0);
            if (select != 0) {
                OnSelect?.Invoke();
            }

            Vector2 navigation = _uiInput.UI.Navigate.ReadValue<Vector2>();
            if (_uiInput.UI.Navigate.WasPressedThisFrame() && navigation != Vector2.zero) {
                OnNavigateUI?.Invoke(navigation);
            }
        }
    }
}
