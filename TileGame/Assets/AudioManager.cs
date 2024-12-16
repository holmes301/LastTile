using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip shiftSFX;
    [SerializeField] private AudioClip clearSFX;
    [SerializeField] private AudioClip placeSFX;
    [SerializeField] private AudioClip navSFX;
    [SerializeField] private AudioClip selectSFX;
    [SerializeField] private AudioClip switchSFX;
    [SerializeField] private AudioClip destroySFX;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private bool _multiplayer = false;
    void OnEnable() {
        if (!_multiplayer) {
            PlayerInputMap.OnShiftBlock += playShiftBlock;
            PlayerInputMap.OnPressedMod += playRotateBlock;
            PlayerInputMap.OnHeldMod += playSwitch;
            TileMovement.OnDestroyTile += playDestroy;
            TileMovement.OnPlaceBlock += playPlaceBlock;
            TileMovement.OnClearRow += playClearRow;
        }
        else {
            TwoPlayerInputMap.OnShiftBlock1P += playShiftBlock;
            TwoPlayerInputMap.OnShiftBlock2P += playShiftBlock;
            TwoPlayerInputMap.OnPressedMod1P += playRotateBlock;
            TwoPlayerInputMap.OnPressedMod2P += playRotateBlock;
            TwoPlayerInputMap.OnHeldMod1P += playSwitch;
            TwoPlayerInputMap.OnHeldMod2P += playSwitch;
            MultiplayerTileMovement.OnPlaceBlock += playPlaceBlock;
            MultiplayerTileMovement.OnClearRow += playClearRow;
            MultiplayerTileMovement.OnDestroyTile += playDestroy;
        }
        UIInputMap.OnNavigateUI += playNavigateUI;
        UIInputMap.OnSelect += playSelectUI;
    }
    void OnDisable() {
        if (!_multiplayer) {
            PlayerInputMap.OnShiftBlock -= playShiftBlock;
            PlayerInputMap.OnPressedMod -= playRotateBlock;
            PlayerInputMap.OnHeldMod -= playSwitch;
            TileMovement.OnDestroyTile -= playDestroy;
            TileMovement.OnPlaceBlock -= playPlaceBlock;
            TileMovement.OnClearRow -= playClearRow;
        }
        else {
            TwoPlayerInputMap.OnShiftBlock1P -= playShiftBlock;
            TwoPlayerInputMap.OnShiftBlock2P -= playShiftBlock;
            TwoPlayerInputMap.OnPressedMod1P -= playRotateBlock;
            TwoPlayerInputMap.OnPressedMod2P -= playRotateBlock;
            TwoPlayerInputMap.OnHeldMod1P -= playSwitch;
            TwoPlayerInputMap.OnHeldMod2P -= playSwitch;
            MultiplayerTileMovement.OnPlaceBlock -= playPlaceBlock;
            MultiplayerTileMovement.OnClearRow -= playClearRow;
            MultiplayerTileMovement.OnDestroyTile -= playDestroy;
        }
        UIInputMap.OnNavigateUI -= playNavigateUI;
        UIInputMap.OnSelect -= playSelectUI;
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
    private void playNavigateUI(Vector2 inputs) {
        if (navSFX is not null) {
            sfxSource.clip = navSFX;
            sfxSource.pitch = 1f;
            sfxSource.volume = 1f;
            sfxSource.Play();
        }
    }
    private void playSelectUI() {
        if (selectSFX is not null) {
            sfxSource.clip = selectSFX;
            sfxSource.pitch = 1f;
            sfxSource.volume = 1f;
            sfxSource.Play();
        }
    }
    private void playSwitch() {
        if (switchSFX is not null) {
            sfxSource.clip = switchSFX;
            sfxSource.pitch = 2.5f;
            sfxSource.volume = 0.5f;
            sfxSource.Play();
        }
    }
    private void playDestroy() {
        if (destroySFX is not null) {
            sfxSource.clip = destroySFX;
            sfxSource.pitch = 1f;
            sfxSource.volume = 0.25f;
            sfxSource.Play();
        }
    }
}
