using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private VirtualJoystick _joystick;
    [SerializeField] private GameConfig _config;
    [SerializeField] private HeadScore _headScore;

    [Header("Settings")]
    [SerializeField] private int _maxSizeStacks = 5;

    private Rigidbody _rb;
    private float _currentSpeed;
    private Vector3 _baseScale;

    // Buff State
    private float _speedBuffTimer;
    private float _sizeBuffTimer;
    private int _currentSizeStack = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        if (_config != null)
        {
            _currentSpeed = _config.PlayerBaseSpeed;
            _baseScale = transform.localScale;
        }
    }

    private void FixedUpdate()
    {
        if (_joystick == null) return;
        Vector3 direction = new Vector3(_joystick.InputDirection.x, 0, _joystick.InputDirection.y);

        if (direction.magnitude > 0.1f)
        {
            Vector3 targetVel = direction * _currentSpeed;
            targetVel.y = _rb.linearVelocity.y;
            _rb.linearVelocity = targetVel;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
    }

    private void Update()
    {
        // Handle Timers
        if (_speedBuffTimer > 0)
        {
            _speedBuffTimer -= Time.deltaTime;
            if (_speedBuffTimer <= 0) _currentSpeed = _config.PlayerBaseSpeed;
        }

        if (_sizeBuffTimer > 0)
        {
            _sizeBuffTimer -= Time.deltaTime;
            if (_sizeBuffTimer <= 0) ResetSize();
        }
    }

    public void ResetStats()
    {
      
        _currentSpeed = _config.PlayerBaseSpeed;
        ResetSize();
        _speedBuffTimer = 0;
        transform.position = new Vector3(0, 1f, 0);

      
        if (_headScore != null)
        {
            
            _headScore.ResetScore();
        }
       
    }

    public void ApplySpeedBuff(float multiplier, float duration)
    {
        _currentSpeed = _config.PlayerBaseSpeed * multiplier;
        _speedBuffTimer = duration;
        if (_headScore != null) _headScore.AddScore(_config.SpeedItemScore);
    }

    public void ApplySizeBuff(float sizeMultiplier, float speedDebuff, float duration)
    {
        if (_currentSizeStack < _maxSizeStacks) _currentSizeStack++;

        float newScaleMult = 1f + ((sizeMultiplier - 1f) * _currentSizeStack);
        transform.localScale = _baseScale * newScaleMult;

        _sizeBuffTimer = duration;
        _currentSpeed = _config.PlayerBaseSpeed * speedDebuff;
        _speedBuffTimer = duration;

        if (_headScore != null) _headScore.AddScore(_config.SizeItemScore);
    }

    private void ResetSize()
    {
        transform.localScale = _baseScale;
        _currentSizeStack = 0;
        _sizeBuffTimer = 0;
    }
}