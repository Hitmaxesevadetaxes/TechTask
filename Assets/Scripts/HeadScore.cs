using TMPro;
using UnityEngine;

public class HeadScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private ScorePresenter _presenter;

    void Start()
    {
        ScoreModel model = new ScoreModel();
        _presenter = new ScorePresenter(model, this);
    }

    public void AddScore(int amount)
    {
        if (_presenter != null) _presenter.AddPoints(amount);
    }

    public void ResetScore()
    {
       

        if (_presenter != null)
        {
            _presenter.Reset();
           
        }
       
    }

    public void UpdateText(string text)
    {
        if (_scoreText != null)
            _scoreText.text = text;
    }

    // Make text face the camera
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
    }
}

//MVP Logic
public class ScoreModel
{
    public int CurrentScore = 0;
}

public class ScorePresenter
{
    private ScoreModel _model;
    private HeadScore _view;

    public ScorePresenter(ScoreModel model, HeadScore view)
    {
        _model = model;
        _view = view;
        UpdateView();
    }

    public void AddPoints(int amount)
    {
        _model.CurrentScore += amount;
        UpdateView();
    }

    public void Reset()
    {
        _model.CurrentScore = 0;
        UpdateView();
    }

    private void UpdateView()
    {
        _view.UpdateText(_model.CurrentScore.ToString());
    }
}