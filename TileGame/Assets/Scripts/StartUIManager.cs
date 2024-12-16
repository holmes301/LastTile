using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> textList;
    private int _selection = 0;
    void OnEnable() {
        UIInputMap.OnNavigateUI += navigateUI;
        UIInputMap.OnSelect += selectUI;
    }
    void OnDisable() {
        UIInputMap.OnNavigateUI -= navigateUI;
        UIInputMap.OnSelect -= selectUI;
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
                // how to play panel here
                break;
            case 3:
                // credits panel here
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
