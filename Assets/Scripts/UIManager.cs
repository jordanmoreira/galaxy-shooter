using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartGameText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score:" + playerScore.ToString();
        _gameOverText.gameObject.SetActive(false);
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives < 1)
        {
            _gameManager.GameOver();
            DisplayGameOver(true);
            StartCoroutine(TextFlicker(_gameOverText));
        }
    }

    public void DisplayGameOver(bool activate)
    {
        if (activate == true)
        {
            _gameOverText.gameObject.SetActive(activate);
            _restartGameText.gameObject.SetActive(true);
        }
    }

    IEnumerator TextFlicker(Text text)
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

}
