using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip shiftSFX;
    [SerializeField] private AudioClip clearSFX;
    [SerializeField] private AudioClip placeSFX;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private bool _multiplayer = false;
    void OnEnable() {
        if (!_multiplayer) {
            PlayerInputMap.OnShiftBlock += playShiftBlock;
            PlayerInputMap.OnPressedMod += playRotateBlock;
            TileMovement.OnPlaceBlock += playPlaceBlock;
            TileMovement.OnClearRow += playClearRow;
        }
        else {
            TwoPlayerInputMap.OnShiftBlock1P += playShiftBlock;
            TwoPlayerInputMap.OnShiftBlock2P += playShiftBlock;
            TwoPlayerInputMap.OnPressedMod1P += playRotateBlock;
            TwoPlayerInputMap.OnPressedMod2P += playRotateBlock;
            MultiplayerTileMovement.OnPlaceBlock += playPlaceBlock;
            MultiplayerTileMovement.OnClearRow += playClearRow;
        }
    }
    void OnDisable() {
        if (!_multiplayer) {
            PlayerInputMap.OnShiftBlock -= playShiftBlock;
            PlayerInputMap.OnPressedMod -= playRotateBlock;
            TileMovement.OnPlaceBlock -= playPlaceBlock;
            TileMovement.OnClearRow -= playClearRow;
        }
        else {
            TwoPlayerInputMap.OnShiftBlock1P -= playShiftBlock;
            TwoPlayerInputMap.OnShiftBlock2P -= playShiftBlock;
            TwoPlayerInputMap.OnPressedMod1P -= playRotateBlock;
            TwoPlayerInputMap.OnPressedMod2P -= playRotateBlock;
            MultiplayerTileMovement.OnPlaceBlock -= playPlaceBlock;
            MultiplayerTileMovement.OnClearRow -= playClearRow;
        }
    }
    private void playShiftBlock(int shift) {
        if (shiftSFX is not null) {
            sfxSource.clip = shiftSFX;
            sfxSource.pitch = 2.5f;
            sfxSource.volume = 1f;
            sfxSource.Play();
        }
    }
    private void playRotateBlock() {
        if (shiftSFX is not null) {
            sfxSource.clip = shiftSFX;
            sfxSource.pitch = 1f;
            sfxSource.volume = 1f;
            sfxSource.Play();
        }
    }
    private void playPlaceBlock() {
        if (placeSFX is not null) {
            sfxSource.clip = placeSFX;
            sfxSource.pitch = 2.5f;
            sfxSource.volume = 0.25f;
            sfxSource.Play();
        }
    }
    private void playClearRow() {
        if (clearSFX is not null) {
            sfxSource.clip = clearSFX;
            sfxSource.pitch = 2.5f;
            sfxSource.volume = 0.25f;
            sfxSource.Play();
        }
    }
}
