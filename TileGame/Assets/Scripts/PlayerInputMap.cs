using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerInputMap : MonoBehaviour
{
    [HideInInspector] public bool active = true;
    [SerializeField] private int minimumFramesToHold = 40;
    public static event Action<int> OnShiftBlock;
    public static event Action OnDropBlock;
    public static event Action OnHeldMod;
    public static event Action OnPressedMod;
    private InputSystem_Actions _playerInput;
    private int _currModifyframes = 0;

    void Awake() {
        _playerInput = new InputSystem_Actions();
    }
    void OnEnable() {
        _playerInput.Player.Enable();
    }
    void OnDisable() {
        _playerInput.Player.Disable();
    }
    void Update() {
        if (active) {
            int shiftBlock = (int) _playerInput.Player.ShiftBlock.ReadValue<float>() * (_playerInput.Player.ShiftBlock.WasPressedThisFrame() ? 1 : 0);
            if (shiftBlock != 0) {
                OnShiftBlock?.Invoke(shiftBlock);
            }

            int dropBlock = (int) _playerInput.Player.DropBlock.ReadValue<float>() * (_playerInput.Player.DropBlock.WasPressedThisFrame() ? 1 : 0);
            if (dropBlock != 0) {
                OnDropBlock?.Invoke();
            }

            int modBlock = (int) _playerInput.Player.ModifyBlock.ReadValue<float>();
            if (modBlock != 0) {
                _currModifyframes++;
                if (_currModifyframes == minimumFramesToHold) {
                    OnHeldMod?.Invoke();
                }
            }
            else if (_playerInput.Player.ModifyBlock.WasCompletedThisFrame() && _currModifyframes < minimumFramesToHold) {
                _currModifyframes = 0;
                OnPressedMod?.Invoke();
            }
            else if (_currModifyframes >= minimumFramesToHold) {
                _currModifyframes = 0;
                // block ended hold
            }
        }
    }
}
