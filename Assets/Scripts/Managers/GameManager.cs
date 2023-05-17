using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    public PlayerController player;

    public bool canMove;
    public static bool _canMove;

    public GameObject deletionThreshold;
    public static GameObject _deletionThreshold;

    private bool coinHitFrame;
    private bool gameStarded;

    public AudioManager audioManager;

    [Header("Character")]
    public GameObject[] characters;
    public GameObject currentChar;

    [Header("SetActive")]
    public GameObject[] NotActiveStart;
    public GameObject[] NotActiveEnd;
    public GameObject DeathPanel;

    [Header("UI")]
    public TMP_Text scoreTXT;
    private float score;

    public TMP_Text bestScoreTXT;
    public TMP_Text NewBestScoreTXT;
    public TMP_Text endScoreTXT;
    private float bestScore;

    public TMP_Text coinsTXT;
    public TMP_Text endCoinsTXT;
    private int coinsTotal;
    private int coinsCollectedPerGame;

    public Button continueButton;

    [Header("Speed")]
    public float worldSpeed;
    public static float _worldSpeed;

    public float IncreaseSpeedTimeDiff;
    public float speedMultiplier;
    public static float _speedMultiplier;

    private float Counter;
    private int continueCounter;


    public void Start()
    {
        WhileStart();

        for (int i = 0; i < characters.Length; i++) //Start the game with choosen char.
        {
            if (characters[i].name == PlayerPrefs.GetString("SelectedChar"))
            {
                currentChar.SetActive(false);

                GameObject newChar = Instantiate(characters[i], player.modelHolder.position, player.modelHolder.rotation);
                newChar.transform.parent = player.modelHolder;

                Destroy(newChar.GetComponent<Rigidbody>());
            }
        }
    }
    public void Update()
    {
        Score();
        IncreaseSpeed();
        WhileUpdate();
        LosePanel();
    }
    private void WhileUpdate()
    {
        coinsTotal = PlayerPrefs.GetInt("Coin");
        bestScore = PlayerPrefs.GetFloat("BestScore");

        _canMove = canMove;
        _worldSpeed = worldSpeed;
        _deletionThreshold = deletionThreshold;
        _speedMultiplier = speedMultiplier;

        coinHitFrame = false;

        coinsTXT.text = coinsTotal.ToString();

        if (continueCounter == 0 && PlayerPrefs.GetInt("Coin") >= 30)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
    }

    private void WhileStart()
    {
        Counter = IncreaseSpeedTimeDiff;
        coinsCollectedPerGame = 0;
        continueCounter = 0;
    }

    public void GameStarts()
    {
        if (!gameStarded)
        {
            canMove = true;
            gameStarded = true;

            for (int i = 0; i < NotActiveStart.Length; i++)
            {
                NotActiveStart[i].SetActive(false);
            }
        }
    }
    private void IncreaseSpeed()
    {
        if (canMove)
        {
            Counter -= Time.deltaTime;

            if (Counter <= 0)
            {
                Counter = IncreaseSpeedTimeDiff;
                worldSpeed *= speedMultiplier;
            }
        }
    }
    public void Hit()
    {
        //interstitialAd.ShowAd();
        canMove = false;
        StartCoroutine(GameEnd());
    }

    private IEnumerator GameEnd()
    {
        audioManager.gameMusic.Stop();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < NotActiveEnd.Length; i++)
        {
            NotActiveEnd[i].SetActive(false);
        }
        DeathPanel.SetActive(true);
        audioManager.gameOver.Play();
    }

    private void LosePanel()
    {
        if (score > bestScore)
        {
            bestScore = score;
            NewBestScoreTXT.text = "NEW BEST SCORE";
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }
        bestScoreTXT.text = ((int)bestScore).ToString();
        endScoreTXT.text = ((int)score).ToString();
        endCoinsTXT.text = ((int)coinsCollectedPerGame).ToString();
    }

    public void CoinCollected()
    {
        if (!coinHitFrame)
        {
            coinsTotal += 1;
            coinsCollectedPerGame += 1;
            coinHitFrame = true;
            PlayerPrefs.SetInt("Coin", coinsTotal);
        }
    }

    private void Score()
    {
        if (canMove)
        {
            score += Time.deltaTime * worldSpeed;
            scoreTXT.text = ((int)score).ToString();
        }
    }
    public void Continue()
    {
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - 30);

        canMove = true;
        _canMove = true;

        DeathPanel.SetActive(false);

        audioManager.gameMusic.Play();
        audioManager.gameOver.Stop();

        for (int i = 0; i < NotActiveEnd.Length; i++)
        {
            NotActiveEnd[i].SetActive(true);
        }
        player.ResetPos();

        continueCounter++;
    }
}
