using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> textList;
    [SerializeField] private GameObject htpPanel;
    [SerializeField] private GameObject creditsPanel;
    private bool _toggleHtp = false;
    private bool _toggleCredits = false;
    private int _selection = 0;
    void OnEnable() {
        UIInputMap.OnNavigateUI += navigateUI;
        UIInputMap.OnSelect += selectUI;
    }
    void OnDisable() {
        UIInputMap.OnNavigateUI -= navigateUI;
        UIInputMap.OnSelect -= selectUI;
    }
    void Start() {
        Time.timeScale = 1.0f;
        if (htpPanel is not null) {
            htpPanel.SetActive(false);
        }
        if (creditsPanel is not null) {
            creditsPanel.SetActive(false);
        }
    }
    private void navigateUI(Vector2 input) {
        if (input.y != 0) {
            modifySelection((int) -input.y);
        }
    }
    private void selectUI() {
        switch (_selection) {
            case 0:
                SceneManager.LoadScene("1P Scene");
                break;
            case 1:
                SceneManager.LoadScene("2P Scene");
                break;
            case 2:
                _toggleHtp = !_toggleHtp;
                if (htpPanel is not null) {
                    htpPanel.SetActive(_toggleHtp);
                }
                break;
            case 3:
                _toggleCredits = !_toggleCredits;
                if (creditsPanel is not null) {
                    creditsPanel.SetActive(_toggleCredits);
                }
                break;
            case 4:
                Application.Quit();
                break;
            default:
                break;
        }
    }
    private void modifySelection(int modification) {
        int futureSelect = (_selection + modification + textList.Count) % textList.Count;
        textList[_selection].color = Color.white;
        textList[_selection].fontStyle = FontStyles.Normal;

        textList[futureSelect].color = Color.green;
        textList[futureSelect].fontStyle = FontStyles.Underline;

        _selection = futureSelect;
    }
}
