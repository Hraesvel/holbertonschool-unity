using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Controllable _ctrl;
    private Vector3 _direction;

    private Gradient _healthColor;
    private Image _healthTextBG;
    private Vector2 _pos;

    private Rigidbody _rigidbody;
    private float _startHealth;
    private Vector2 _startPos;
    public Image _winLoseBg;

    [Tooltip("Allow the Health Background color to shift based on the gradient")]
    public bool allowHpClrChange = true;


    [SerializeField] private int health = 5;

    public Color healthEnd = Color.black;

    public Color healthStart = Color.red;
    public Text healthText;

    private int score;

    public Text scoreText;

    public float speed = 1;
    public Image touchBG;
    public Image touchMove;
    public Text winLoseText;

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _healthTextBG = healthText.transform.parent.GetComponent<Image>();
        _winLoseBg = winLoseText.transform.parent.GetComponent<Image>();

        _pos = new Vector2(0f, 0f);
        _startPos = new Vector2(0f, 0f);
        _healthColor = new Gradient();

        var colorKey = new GradientColorKey[2];
        colorKey[0].color = healthStart;
        colorKey[0].time = 1f;
        colorKey[1].color = healthEnd;
        colorKey[1].time = 0f;
        _healthColor.colorKeys = colorKey;

        _winLoseBg.gameObject.SetActive(false);

        scoreText.text = string.Format("Score: {0}", score);
        healthText.text = string.Format("Health: {0}", Health);
        _healthTextBG.color = _healthColor.Evaluate(1);
        _startHealth = Health;

        touchBG.gameObject.SetActive(false);
        touchMove.gameObject.SetActive(false);

        _ctrl = new Controllable {StartImage = touchBG, ToImage = touchMove, StartPos = _startPos, ToPos = _pos};

        if (PlayerPrefs.HasKey("touchSensitivity"))
            _ctrl.Sensitivity = PlayerPrefs.GetFloat("touchSensitivity");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("menu");

        if (Health == 0)
        {
            winLoseText.text = "Game Over!";
            winLoseText.color = Color.white;
            _winLoseBg.color = Color.red;
            _winLoseBg.gameObject.SetActive(true);
            StartCoroutine(LoadScene(3.0f));
            // Debug.Log("Game Over!");
        }
    }

    private void FixedUpdate()
    {
        if (UpdateDirection(ref _direction))
            _rigidbody.AddForce(_direction * speed);
        else
            _direction.Set(0, 0, 0);
    }

    private bool UpdateDirection(ref Vector3 dir)
    {
        return _ctrl.TouchController(ref dir) || _ctrl.KeyController(ref dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score += 1;
            SetScoreText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            Health -= 1;
            SetHealthText();
        }

        if (other.CompareTag("Goal"))
        {
            winLoseText.text = "You Win!";
            winLoseText.color = Color.black;
            _winLoseBg.color = Color.green;
            _winLoseBg.gameObject.SetActive(true);
            StartCoroutine(LoadScene(3.0f));


            // Debug.Log("You win!");
        }
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + score;
        // Debug.Log("Score: " + score);
    }

    private void SetHealthText()
    {
        healthText.text = "Health: " + Health;
        if (allowHpClrChange)
            _healthTextBG.color = _healthColor.Evaluate(Health / _startHealth);
        // Debug.Log("Health: " + health);
    }

    private IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("maze");
    }
}