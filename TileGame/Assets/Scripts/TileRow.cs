using System;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    public bool isEmpty;
    public float speed = 1f;
    public int density = 8;
    [SerializeField] private GameObject tile;
    [SerializeField] private bool needsStart = false;
    [SerializeField] private int rowSize = 10;
    [HideInInspector] public Vector3 rowPosition;
    private List<GameObject> _tilesInRow = new List<GameObject>();
    private bool _clearing = false;

    void Start() {
        if (needsStart) {
            if (rowSize > 0 && tile is not null) {
                for (int i = 0; i < rowSize && !isEmpty; i++) {
                    if (UnityEngine.Random.Range(0, 10) < density && _tilesInRow.Count < rowSize - 1) { // density makes it more likely tiles will appear
                        GameObject gc = Instantiate(tile, this.transform.position + new Vector3(tile.transform.localScale.x * i, 0, 0), Quaternion.identity);
                        gc.transform.parent = this.transform;
                        _tilesInRow.Add(gc);
                    }
                }
            }
        }
    }
    void Update() {
        if (!_clearing) {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        rowPosition = transform.position;
    }

    public void PauseClearing() => _clearing = true;
    public void ResumeClearing() => _clearing = false;

    
    public void ManualInitialize() {
        for (int i = 0; i < rowSize && !isEmpty && !needsStart; i++) {
            if (UnityEngine.Random.Range(0, 10) < density && _tilesInRow.Count < rowSize - 1) { // density makes it more likely tiles will appear
                GameObject gc = Instantiate(tile, this.transform.position + new Vector3(tile.transform.localScale.x * i, 0, 0), Quaternion.identity);
                gc.transform.parent = this.transform;
                _tilesInRow.Add(gc);
            }
        }
    }
    public void AddTile(float xPos) {
        GameObject gc = Instantiate(tile, new Vector3(xPos, transform.position.y, transform.position.z), Quaternion.identity);
        gc.transform.parent = this.transform;
        _tilesInRow.Add(gc);
    }
    public void RemoveAllTiles() {
        foreach (GameObject gc in _tilesInRow) {
            if (gc is not null) {
                Destroy(gc);
            }
        }
        _tilesInRow.Clear();
    }
    public List<float> GetAllTiles() {
        List<float> tiles = new List<float>();
        foreach (GameObject gc in _tilesInRow) {
            tiles.Add(gc.transform.position.x);
        }
        return tiles;
    }
    public bool CheckNeedsClear() {
        return _tilesInRow.Count == rowSize;
    }
}
