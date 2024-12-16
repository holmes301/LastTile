using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool multiplayer = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text resumeText;
    [SerializeField] private TMP_Text exitText;
    [SerializeField] private TMP_Text restartText;
    [SerializeField] private TMP_Text exitTextGameOver;
    [SerializeField] private TMP_Text restartTextGameOver;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2P;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text levelDisplay;
    [SerializeField] private GameObject levelUpObject;
    public static event Action OnSelectResume;
    private bool _paused = false;
    private bool _gameOver = false;
    private int _selection = 0;
    private int _score = 0;
    private int _score2P = 0;
    private int _level = 0;
    void OnEnable() {
        UIInputMap.OnPauseGame += pauseGame;
        UIInputMap.OnNavigateUI += navigateUI;
        UIInputMap.OnSelect += selectUI;

        if (!multiplayer) {
            TileMovement.OnScoreChange += changeScore;
            TileMovement.OnGameOver += toggleGameOver;
        }
        else {
            MultiplayerTileMovement.OnScoreChange += changeScore;
            MultiplayerTileMovement.OnGameOver += toggleGameOver;
        } 

        GameManager.OnLevelUp += updateLevel;
    }
    void OnDisable() {
        UIInputMap.OnPauseGame -= pauseGame;
        UIInputMap.OnNavigateUI -= navigateUI;
        UIInputMap.OnSelect -= selectUI;

        if (!multiplayer) {
            TileMovement.OnScoreChange -= changeScore;
            TileMovement.OnGameOver -= toggleGameOver;
        }
        else {
            MultiplayerTileMovement.OnScoreChange -= changeScore;
            MultiplayerTileMovement.OnGameOver -= toggleGameOver;
        } 


        GameManager.OnLevelUp -= updateLevel;
    }
    void Awake() {
        pausePanel.SetActive(false);
        levelUpObject.SetActive(false);
        gameOverPanel.SetActive(false);
    }
    private void pauseGame() {
        if (!_gameOver) {
            _paused = !_paused;
            pausePanel.SetActive(_paused);
            if (_paused) {
                Time.timeScale = 0.0f;
            }
            else {
                Time.timeScale = 1.0f;
            }
        }
    }
    private void toggleGameOver() {
        _gameOver = true;
        _paused = false;
        _selection = 0;
        gameOverPanel.SetActive(true);
        levelUpObject.SetActive(false);
    }
    private void navigateUI(Vector2 input) {
        if (input.y != 0) {
            modifySelection(_selection, (int) -input.y);
        }
    }
    private void selectUI() {
        if (!_gameOver) {
            if (_selection == 0) { // resume
                pauseGame();
                OnSelectResume?.Invoke();
            }
            else if (_selection == 1) { // restart
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else { // exit
                SceneManager.LoadScene("Start Scene");
            }
        }
        else {
            if (_selection == 0) {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else {
                SceneManager.LoadScene("Start Scene");
            }
        }
    }
    private void modifySelection(int current, int modification) {
        if (!_gameOver) {
            if (_selection == 0) { // resume
                resumeText.color = Color.white;
                resumeText.fontStyle = FontStyles.Normal;
                _selection = (current + modification + 3) % 3;
                if (_selection == 1) {
                    restartText.color = Color.green;
                    restartText.fontStyle = FontStyles.Underline;
                }
                else {
                    exitText.color = Color.green;
                    exitText.fontStyle = FontStyles.Underline;
                }
            }
            else if (_selection == 1) { // restart
                restartText.color = Color.white;
                restartText.fontStyle = FontStyles.Normal;
                _selection = (current + modification + 3) % 3;
                if (_selection == 0) {
                    resumeText.color = Color.green;
                    resumeText.fontStyle = FontStyles.Underline;
                }
                else {
                    exitText.color = Color.green;
                    exitText.fontStyle = FontStyles.Underline;
                }
            }
            else { // exit
                exitText.color = Color.white;
                exitText.fontStyle = FontStyles.Normal;
                _selection = (current + modification + 3) % 3;
                if (_selection == 0) {
                    resumeText.color = Color.green;
                    resumeText.fontStyle = FontStyles.Underline;
                }
                else {
                    restartText.color = Color.green;
                    restartText.fontStyle = FontStyles.Underline;
                }
            }
        }
        else {
            if (_selection == 0) { // restart
                restartTextGameOver.color = Color.white;
                restartTextGameOver.fontStyle = FontStyles.Normal;
                _selection = (current + modification + 2) % 2;
                exitTextGameOver.color = Color.green;
                exitTextGameOver.fontStyle = FontStyles.Underline;
            }
            else { // exit
                exitTextGameOver.color = Color.white;
                exitTextGameOver.fontStyle = FontStyles.Normal;
                _selection = (current + modification + 2) % 2;
                restartTextGameOver.color = Color.green;
                restartTextGameOver.fontStyle = FontStyles.Underline;
            }
        }
    }

    private void changeScore(int scoreToAdd) {
        if (!multiplayer) {
            _score += scoreToAdd;
            scoreText.text = _score.ToString();
        }
        else {
            if (scoreToAdd > 0) {
                _score += scoreToAdd;
                scoreText.text = "SCORE: " + _score + "\n < < <";
            }
            else {
                _score2P += -scoreToAdd;
                scoreText2P.text = "SCORE: " + _score2P + "\n > > >";
            }
        }
    }

    private void updateLevel(Vector2 updates) {
        _level++;
        if (levelDisplay is not null) {
            levelDisplay.text = _level.ToString();
        }
        StartCoroutine(displayLevelUp(updates.x > 0));
    }
    private IEnumerator displayLevelUp(bool density) {
        if (density) {
            levelText.text = "- - TILE DENSITY DECREASED - -";
            levelText.color = Color.red;
        }
        else {
            levelText.text = "- - SPEED INCREASED - -";
            levelText.color = Color.green;
        }
        levelUpObject.SetActive(true);
        yield return new WaitForSeconds(4);
        levelUpObject.SetActive(false);
    }
}
