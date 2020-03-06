using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    private Vector3 _direction;

    public Canvas hud;

    private int score = 0;
    public int health = 5;
    private float _startHealth;

    public Text scoreText;
    public Text healthText;
    public Text winLoseText;
    public Image _winLoseBg;

    private Image _healthTextBG;
    private Gradient _healthColor;

    [Tooltip("Allow the Health Background color to shift based on the gradient")]
    public bool allowHpClrChange = true;

    public Color healthStart = Color.red;
    public Color healthEnd = Color.black;


    // Start is called before the first frame update
    void Start()
    {
        _healthColor = new Gradient();

        var colorkey = new GradientColorKey[2];
        colorkey[0].color = healthStart;
        colorkey[0].time = 1f;
        colorkey[1].color = healthEnd;
        colorkey[1].time = 0f;
        _healthColor.colorKeys = colorkey;
        _healthTextBG = healthText.transform.parent.GetComponent<Image>();
        _winLoseBg = winLoseText.transform.parent.GetComponent<Image>();


        _winLoseBg.gameObject.SetActive(false);

        scoreText.text = "Score: " + score;
        healthText.text = "Health: " + health;
        _healthTextBG.color = _healthColor.Evaluate(1);
        _startHealth = health;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("menu");

        if (health == 0)
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
        _direction.Set(0, 0, 0);

        if (UpdateDirection(ref _direction))
            GetComponent<Rigidbody>().AddForce((_direction * speed));
    }

    private static bool UpdateDirection(ref Vector3 dir)
    {
        if (!Input.anyKey) return false;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x += 1;
        }


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x -= 1;
        }


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.z += 1;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.z -= 1;
        }

        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score += 1;
            SetScoreText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            health -= 1;
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

    void SetScoreText()
    {
        scoreText.text = "Score: " + score;
        // Debug.Log("Score: " + score);
    }

    void SetHealthText()
    {
        healthText.text = "Health: " + health;
        if (allowHpClrChange)
            _healthTextBG.color = _healthColor.Evaluate(health / _startHealth);
        // Debug.Log("Health: " + health);
    }

    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("maze");
    }
}