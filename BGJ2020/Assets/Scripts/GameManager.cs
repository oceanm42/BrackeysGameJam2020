using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool gameHasEnded;
    public Animator fasterAnimator;
    private bool playerWon;
    private Rigidbody2D playerRb;

    [Header("End Game Messages")]
    [SerializeField]
    private string[] victoryMessages;
    [SerializeField]
    private string[] lossMessages;

    [SerializeField]
    private Animator camAnimator;

    [SerializeField]
    private TextMeshProUGUI endGameText;
    [SerializeField]
    private TextMeshProUGUI instructionText;
    public TextMeshProUGUI fasterText;

    [Header("Time Settings")]
    [SerializeField]
    private Slider timeLeftSlider;
    [SerializeField]
    private float maxTime;
    private float currentTime;

    [Header("Particle Settings")]
    public ParticleSystem particles;
    public Sprite ff;
    public Sprite rw;


    private void Start()
    {
        playerWon = false;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        currentTime = maxTime;
        timeLeftSlider.maxValue = maxTime;
    }

    public void EndGame(bool gameWon)
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            if (gameWon == true)
            {
                playerWon = true;
                playerRb.velocity = Vector2.zero;
                StartCoroutine(WriteText(.1f, victoryMessages[Random.Range(0, victoryMessages.Length)], endGameText));
                StartCoroutine(WriteText(.05f, "PRESS 'ENTER' TO CONTINUE", instructionText));
            }
            else
            {
                StartCoroutine(WriteText(.1f, lossMessages[Random.Range(0, lossMessages.Length)], endGameText));
                StartCoroutine(WriteText(.05f, "PRESS 'ENTER' TO RESTART", instructionText));
            }
        }
    }

    private void Update()
    {
        if (gameHasEnded == false)
        {
            currentTime -= 1 * Time.deltaTime;
            timeLeftSlider.value = currentTime;
            if (currentTime <= 0)
            {
                EndGame(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (playerWon == true)
                {
                    Debug.Log("Loading next scene");
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    public void ChangeCameraColor()
    {
        if (camAnimator.GetBool("isRewinding") == true)
        {
            camAnimator.SetBool("isRewinding", false);
        }
        else
        {
            camAnimator.SetBool("isRewinding", true);
        }
    }

    public IEnumerator WriteText(float textDelay, string text, TextMeshProUGUI gameText)
    {
        string newText = "";
        for (int i = 0; i < text.Length; i++)
        {
            newText += text[i];
            yield return new WaitForSeconds(textDelay);
            gameText.text = newText;
        }
    }
}
