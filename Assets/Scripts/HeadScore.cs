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

    // Simplified: rotate only the text to face the camera, keep it upright
    void LateUpdate()
    {
        if (Camera.main == null || _scoreText == null) return;

        Vector3 target = Camera.main.transform.position;
        // keep label upright by matching its Y
        target.y = _scoreText.transform.position.y;

        // Make text face the camera
        _scoreText.transform.LookAt(target);

        // TMP faces backwards by default in many setups — flip to face camera
        _scoreText.transform.Rotate(0f, 180f, 0f);
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