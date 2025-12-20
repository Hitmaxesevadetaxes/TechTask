using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameHUD;
    [SerializeField] private GameObject _endPanel;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    // We track the score locally for display
    private int _displayScore = 0;

    private void Start()
    {
        ShowMenu();
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += HandleGameStart;

        
        GameEvents.OnGameRestart += HandleGameStart;

        GameEvents.OnScoreChanged += UpdateScoreUI;
        GameEvents.OnAllTargetsCollected += HandleGameEnd;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;

        
        GameEvents.OnGameRestart -= HandleGameStart;

        GameEvents.OnScoreChanged -= UpdateScoreUI;
        GameEvents.OnAllTargetsCollected -= HandleGameEnd;
    }

    // BUTTON FUNCTIONS 
    public void OnStartButtonPressed()
    {
        GameEvents.OnGameStart?.Invoke();
    }

    public void OnRestartButtonPressed()
    {
        GameEvents.OnGameRestart?.Invoke();
    }

    //EVENT HANDLERS 
    private void ShowMenu()
    {
        if (_menuPanel) _menuPanel.SetActive(true);
        if (_gameHUD) _gameHUD.SetActive(false);
        if (_endPanel) _endPanel.SetActive(false);
    }

    private void HandleGameStart()
    {
        if (_menuPanel) _menuPanel.SetActive(false);
        if (_endPanel) _endPanel.SetActive(false);
        if (_gameHUD) _gameHUD.SetActive(true);

        // Reset score display on start
        _displayScore = 0;
        UpdateTextDisplay();
    }

    private void HandleGameEnd()
    {
        if (_gameHUD) _gameHUD.SetActive(false);
        if (_endPanel) _endPanel.SetActive(true);
    }

    
    private void UpdateScoreUI(int amountAdded)
    {
        _displayScore += amountAdded;
        UpdateTextDisplay();
    }

    private void UpdateTextDisplay()
    {
        if (_scoreText != null)
        {
            _scoreText.text = "Score: " + _displayScore;
        }
    }
}