using System;
using UnityEngine;

public class TwoPlayerInputMap : MonoBehaviour
{
    [HideInInspector] public bool active = true;
    [SerializeField] private int minimumFramesToHold1P = 40;
    [SerializeField] private int minimumFramesToHold2P = 40;
    public static event Action<int> OnShiftBlock1P;
    public static event Action<int> OnShiftBlock2P;
    public static event Action OnDropBlock1P;
    public static event Action OnDropBlock2P;
    public static event Action OnHeldMod1P;
    public static event Action OnHeldMod2P;
    public static event Action OnPressedMod1P;
    public static event Action OnPressedMod2P;
    private InputSystem_Actions _playerInput;
    private int _currModifyFrames1P = 0;
    private int _currModifyFrames2P = 0;

    void Awake() {
        _playerInput = new InputSystem_Actions();
    }
    void OnEnable() {
        _playerInput._1P.Enable();
        _playerInput._2P.Enable();
    }
    void OnDisable() {
        _playerInput._1P.Disable();
        _playerInput._2P.Disable();
    }
    void Update() {
        if (active) {
            // FIRST PLAYER
            int shiftBlock1P = (int) _playerInput._1P.ShiftBlock.ReadValue<float>() * (_playerInput._1P.ShiftBlock.WasPressedThisFrame() ? 1 : 0);
            if (shiftBlock1P != 0) {
                OnShiftBlock1P?.Invoke(shiftBlock1P);
            }

            int dropBlock1P = (int) _playerInput._1P.DropBlock.ReadValue<float>() * (_playerInput._1P.DropBlock.WasPressedThisFrame() ? 1 : 0);
            if (dropBlock1P != 0) {
                OnDropBlock1P?.Invoke();
            }

            int modBlock1P = (int) _playerInput._1P.ModifyBlock.ReadValue<float>();
            if (modBlock1P != 0) {
                _currModifyFrames1P++;
                if (_currModifyFrames1P == minimumFramesToHold1P) {
                    OnHeldMod1P?.Invoke();
                }
            }
            else if (_playerInput._1P.ModifyBlock.WasCompletedThisFrame() && _currModifyFrames1P < minimumFramesToHold1P) {
                _currModifyFrames1P = 0;
                OnPressedMod1P?.Invoke();
            }
            else if (_currModifyFrames1P >= minimumFramesToHold1P) {
                _currModifyFrames1P = 0;
                // block ended hold
            }

            // SECOND PLAYER
            int shiftBlock2P = (int) _playerInput._2P.ShiftBlock.ReadValue<float>() * (_playerInput._2P.ShiftBlock.WasPressedThisFrame() ? 1 : 0);
            if (shiftBlock2P != 0) {
                OnShiftBlock2P?.Invoke(shiftBlock2P);
            }

            int dropBlock2P = (int) _playerInput._2P.DropBlock.ReadValue<float>() * (_playerInput._2P.DropBlock.WasPressedThisFrame() ? 1 : 0);
            if (dropBlock2P != 0) {
                OnDropBlock2P?.Invoke();
            }

            int modBlock2P = (int) _playerInput._2P.ModifyBlock.ReadValue<float>();
            if (modBlock2P != 0) {
                _currModifyFrames2P++;
                if (_currModifyFrames2P == minimumFramesToHold2P) {
                    OnHeldMod2P?.Invoke();
                }
            }
            else if (_playerInput._2P.ModifyBlock.WasCompletedThisFrame() && _currModifyFrames2P < minimumFramesToHold2P) {
                _currModifyFrames2P = 0;
                OnPressedMod2P?.Invoke();
            }
            else if (_currModifyFrames2P >= minimumFramesToHold2P) {
                _currModifyFrames2P = 0;
                // block ended hold
            }
        }
    }
}
