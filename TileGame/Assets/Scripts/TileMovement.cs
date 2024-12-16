using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TileMovement : MonoBehaviour
{
    public static event Action<int> OnScoreChange;
    public static event Action OnGameOver;
    public static event Action OnPlaceBlock;
    public static event Action OnClearRow;
    [SerializeField] private UnityEngine.Vector2 leftRightBounds;
    [SerializeField] private GameObject rowfab;
    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private Transform tileSpawn;
    [SerializeField] private float tileSpeed = 10f;
    [SerializeField] private List<TileRow> rows;
    [SerializeField] private float tileOffset = 0.5f;
    [SerializeField] private float yMaxThreshold;
    [SerializeField] private Transform rowParent;
    [SerializeField] private float groundPos = -4.75f;
    private GameObject _currentTile;
    private TileRow _previousRow;
    private bool _build = false;
    private bool _tileCanMove = false;
    private bool _gameOverInvoked = false;
    private int _tileIndex = 7;
    private int _tileDensity = 8;
    private float _tileSpeed = 0.1f;
    void OnEnable() {
        // INPUT
        PlayerInputMap.OnShiftBlock += shiftBlock;
        PlayerInputMap.OnDropBlock += dropBlock;
        PlayerInputMap.OnHeldMod += heldModify;
        PlayerInputMap.OnPressedMod += pressedModify;

        GameManager.OnLevelUp += levelUp;
    }
    void OnDisable() {
        //INPUT
        PlayerInputMap.OnShiftBlock -= shiftBlock;
        PlayerInputMap.OnDropBlock -= dropBlock;
        PlayerInputMap.OnHeldMod -= heldModify;
        PlayerInputMap.OnPressedMod -= pressedModify;

        GameManager.OnLevelUp -= levelUp;
    }
    void Start() {
        generateNewTile();
        if (rows.Count > 0) {
            _previousRow = rows[0];
        }
    }
    void Update() {
        if (_tileCanMove && _currentTile is not null) {
            _currentTile.transform.position += new UnityEngine.Vector3(0, -tileSpeed * Time.deltaTime, 0);
            foreach (Transform child in _currentTile.transform) {
                Collider2D[] hits = Physics2D.OverlapCircleAll(child.position, (child.localScale.x / 2) - 0.05f);
                foreach(var hit in hits) {
                    if (hit.transform.parent != child.parent) {
                        _tileCanMove = false;
                        snapTiles();
                        generateNewTile();
                        break;
                    }
                }
                if (!_tileCanMove) break;
            }
        }
        if (_previousRow != null && groundPos < _previousRow.gameObject.transform.position.y - 0.5f) {
            GameObject go = Instantiate(rowfab);
            go.transform.position = _previousRow.gameObject.transform.position - new UnityEngine.Vector3(0, 0.49f, 0);
            go.name = "Row (" + _tileIndex + ")";
            if (rowParent is not null) go.transform.parent = rowParent;
            _tileIndex++;
            rows.Add(go.GetComponent<TileRow>());
            _previousRow = go.GetComponent<TileRow>();;
            go.GetComponent<TileRow>().density = _tileDensity;
            go.GetComponent<TileRow>().speed = _tileSpeed;
            go.GetComponent<TileRow>().ManualInitialize();
        }

        for (int i = rows.Count - 1; i >= 0; i--) {
            TileRow tr = rows[i];
            if (tr.rowPosition.y > yMaxThreshold) {
                if (tr.GetAllTiles().Count > 0 && !_gameOverInvoked) {
                    OnGameOver?.Invoke();
                    _gameOverInvoked = true;
                }
                else {
                    Destroy(tr.gameObject);
                    rows.RemoveAt(i);
                }
            }
        }
        checkForClear();
    }
    private void checkForClear() {
    for (int i = rows.Count - 1; i >= 0; i--) {
        if (rows[i].CheckNeedsClear()) {
            rowClear(rows[i].rowPosition.y);
        }
    }
}
    private void shiftBlock(int direction) {
        if (_currentTile != null) {
            bool canShift = true;
            foreach (Transform child in _currentTile.transform) {
                if (child.transform.position.x + direction < leftRightBounds.x || child.transform.position.x + direction > leftRightBounds.y) {
                    canShift = false;
                    break;
                }
            }
            _currentTile.transform.position += canShift? new UnityEngine.Vector3(direction * 0.5f, 0, 0) : UnityEngine.Vector3.zero;
        }
    }
    private void dropBlock() {
        _tileCanMove = true;
    }
    private void heldModify() {
        _build = !_build;
    }
    private void pressedModify() {
        if (_currentTile != null) {
            _currentTile.transform.Rotate(0, 0, 90);
        }
    }
    private void generateNewTile() {
        if (_currentTile is not null) {
            Destroy(_currentTile);
            _currentTile = null;
        }
        if (tileList is not null && tileList.Count > 0) {
            int r = UnityEngine.Random.Range(0, tileList.Count);
            _currentTile = Instantiate(tileList[r], tileSpawn);
        }
    }
    private void snapTiles() {
        if (_currentTile is not null) {
            int rowsCleared = 0;
            List<GameObject> tilesToDestroy = new List<GameObject>();
            Dictionary<float, TileRow> rowsToClear = new Dictionary<float, TileRow>();
            foreach (Transform child in _currentTile.transform) {
                foreach (TileRow tr in rows) {
                    if (Mathf.Abs(child.transform.position.y - tr.rowPosition.y) < child.transform.localScale.x / 2) {
                        tr.AddTile(child.transform.position.x);
                        tilesToDestroy.Add(child.gameObject);
                        if (tr.CheckNeedsClear() && !rowsToClear.ContainsKey(tr.rowPosition.y)) {
                            rowsToClear[tr.rowPosition.y] = tr;
                        }
                        break;
                    }
                }
            }
            foreach (GameObject tile in tilesToDestroy) {
                Destroy(tile);
            }
            foreach (var row in rowsToClear) {
                rowClear(row.Key);
                rowsCleared++;
            }
            if (rowsToClear.Count == 0) OnPlaceBlock?.Invoke();
            OnScoreChange?.Invoke((rowsCleared * 400) + 50);
        }
    }
    private void levelUp(UnityEngine.Vector2 updates) {
        _tileDensity -= (int) updates.x;
        _tileSpeed += updates.y;
        foreach (TileRow tr in rows) {
            tr.speed = _tileSpeed;
        }
    }

    private void rowClear(float row) {
        int rowSubject = -1;
        rows.Sort((a, b) => b.rowPosition.y.CompareTo(a.rowPosition.y));
        for (int i = 0; i < rows.Count; i++) {
            if (rows[i].rowPosition.y >= row - 0.1f) {
                rowSubject = i;
            }
        }
        for (int i = rowSubject; i >= 0; i--) {
            List<float> rowTiles = new List<float>();
            if (i - 1 >= 0) {
                rowTiles = rows[i - 1].GetAllTiles();
                removeAndRegenerate(rows[i], rowTiles);
            }
            else {
                rows[i].RemoveAllTiles();
            }
        }
        OnClearRow?.Invoke();
    }
    private void removeAndRegenerate(TileRow tr, List<float> tiles) {
        tr.RemoveAllTiles();
        foreach (float tilePos in tiles) {
            tr.AddTile(tilePos);
        }
    }
}
