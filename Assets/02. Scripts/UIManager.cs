using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameEvent
{
    public Text timerText;
    public Text scoreText;

    public GameObject gameMenuUI;
    public GameObject[] gamePlayUI;

    [Title("ReadyUI")]
    public GameObject gameReadyUI;
    public Text gameReadyText;

    [Space]
    [Title("EndUI")]
    public GameObject gameEndUI;
    public Text newRecordText;
    public Text nowScoreText;
    public Text bestScoreText;
    public Text rankText;

    [Space]
    [Title("OptionUI")]
    public GameObject gameOptionUI;

    [Space]
    [Title("Value")]
    [SerializeField]
    private float score = 0;

    [Title("Corution")]
    public IEnumerator readyTimerCorution;
    public IEnumerator timerCoroutine;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1);


    private void Awake()
    {
        GameManager.GameStart += this.GameStart;
        GameManager.GamePause += this.GamePause;
        GameManager.GameEnd += this.GameEnd;

        GameManager.PlusScore += this.PlusScore;
        GameManager.MinusScore += this.MinusScore;

        timerText.text = "";
        scoreText.text = "";

        gameMenuUI.SetActive(false);

        for(int i = 0; i < gamePlayUI.Length; i ++)
        {
            gamePlayUI[i].SetActive(false);
        }

        gameReadyUI.SetActive(false);

    }

    private void Start()
    {
        readyTimerCorution = ReadyTimerCorution(ValueManager.instance.GetReadyTimer());
        timerCoroutine = TimerCorution(ValueManager.instance.GetTimer());
    }

    private void OnApplicationQuit()
    {
        GameManager.GameStart -= this.GameStart;
        GameManager.GamePause -= this.GamePause;
        GameManager.GameEnd -= this.GameEnd;

        GameManager.PlusScore -= this.PlusScore;
        GameManager.MinusScore -= this.MinusScore;
    }

    public void OpenMenu()
    {
        gameMenuUI.SetActive(true);
    }

    public void CloseMenu()
    {
        gameMenuUI.SetActive(false);
    }

    public void OpenGamePlayUI(GamePlayType type)
    {
        gamePlayUI[(int)type].SetActive(true);
    }

    public void GameStart()
    {
        StartCoroutine(readyTimerCorution);
    }

    public void GamePause()
    {

    }

    public void GameEnd()
    {
        StopCoroutine(timerCoroutine);
    }

    public void PlusScore(int index)
    {
        score += index;
        scoreText.text = "SCORE : " + score.ToString();
    }

    public void MinusScore(int index)
    {
        score = (score - index >= 0) ? score -= index : score = 0;
        scoreText.text = "SCORE : " + score.ToString();
    }

    #region Corution

    private IEnumerator ReadyTimerCorution(float time)
    {
        float number = time;

        gameReadyUI.SetActive(true);

        while (number > 0)
        {
            number -= 1;
            gameReadyText.text = number.ToString();

            yield return waitForSeconds;
        }

        gameReadyUI.SetActive(false);

        StartCoroutine(timerCoroutine);
    }

    private IEnumerator TimerCorution(float time)
    {
        float number = time;

        while(number > 0)
        {
            number -= 1;
            timerText.text = number.ToString();

            yield return waitForSeconds;
        }
    }

    #endregion
}
