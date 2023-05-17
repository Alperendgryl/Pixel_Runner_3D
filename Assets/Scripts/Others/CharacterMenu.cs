using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour
{
    public GameObject[] characters;
    private int currentChar;

    [Header("UnableToBuy")]
    public GameObject disabled;
    public GameObject notEnoughCoins;

    [Header("Buttons")]
    public GameObject Play;
    public GameObject Purchase;

    [Header("Coin")]
    public TMP_Text coinTXT;
    public GameObject unlockCharCoin;
    public int charCost;
    private int totalCoin;

    [Header("Camera")]
    public new Transform camera;
    public float camLerpSpeed;
    private Vector3 camTargetPos;
    void Start()
    {
        onStart();
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) //Lock all chars
        {
            for (int i = 1; i < characters.Length; i++)
            {
                PlayerPrefs.SetInt(characters[i].name, 0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.U)) //Unlock all chars
        {
            for (int i = 0; i < characters.Length; i++)
            {
                PlayerPrefs.SetInt(characters[i].name, 1);
            }
        }
        #endif

        onUpdate();
        UnlockedCheck();
    }

    private void onStart()
    {
        currentChar = 0;
        camTargetPos = camera.position;

        PlayerPrefs.SetInt(characters[0].name, 1);

        if (PlayerPrefs.HasKey("Coin"))
        {
            totalCoin = PlayerPrefs.GetInt("Coin");
        }
        else
        {
            PlayerPrefs.SetInt("Coin", 0); //for initial start
        }
    }

    private void onUpdate()
    {
        totalCoin = PlayerPrefs.GetInt("Coin");
        coinTXT.text = PlayerPrefs.GetInt("Coin").ToString();

        camera.position = Vector3.Lerp(camera.position, camTargetPos, camLerpSpeed * Time.deltaTime);
    }


    private void ChangeCharacter(string buttonSide)
    {
        if (currentChar < characters.Length - 1 && buttonSide.Equals("Right"))
        {
            camTargetPos += new Vector3(4f, 0, 0);
            currentChar++;
        }
        else if (currentChar > 0 && buttonSide.Equals("Left"))
        {
            camTargetPos -= new Vector3(4f, 0, 0);
            currentChar--;
        }
    }

    private IEnumerator coinAnim()
    {
        Debug.Log("Not enough money");

        coinTXT.color = Color.red;
        unlockCharCoin.GetComponent<Animator>().enabled = true;
        notEnoughCoins.SetActive(true);

        yield return new WaitForSeconds(1f);

        coinTXT.color = Color.black;
        unlockCharCoin.GetComponent<Animator>().enabled = false;
        notEnoughCoins.SetActive(false);
    }
    public void UnlockCharacter()
    {
        if (totalCoin >= charCost)
        {
            PlayerPrefs.SetInt("Coin", totalCoin - charCost);
            PlayerPrefs.SetInt(characters[currentChar].name, 1);
            Debug.Log("Purchase Successfully");
        }
        else
        {
            StartCoroutine(coinAnim());
        }
    }

    private void UnlockedCheck()
    {
        if (PlayerPrefs.HasKey(characters[currentChar].name))
        {
            if (PlayerPrefs.GetInt(characters[currentChar].name) == 0) //not unlocked
            {
                Play.SetActive(true);
                Play.GetComponent<Button>().interactable = false;

                Purchase.SetActive(true);
                Purchase.GetComponent<Button>().interactable = true;

                disabled.SetActive(true);
            }
            else
            {
                Play.SetActive(true);
                Play.GetComponent<Button>().interactable = true;

                Purchase.SetActive(true);
                Purchase.GetComponent<Button>().interactable = false;

                disabled.SetActive(false);
            }
        }
        else //unlocked
        {
            PlayerPrefs.SetInt(characters[currentChar].name, 0);
            UnlockedCheck();
        }
    }
    public void SelectAndPlay()
    {
        PlayerPrefs.SetString("SelectedChar", characters[currentChar].name);
        SceneManager.LoadScene("Main"); 
    }
}

