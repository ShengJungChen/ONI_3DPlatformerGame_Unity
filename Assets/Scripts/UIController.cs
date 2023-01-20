using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text chickenCount;
    public Text stageText;
    public Text resultText;
    public Text resultTimeText;
    public Button replayBtn;
    public Button pauseBtn;
    public Button resumeBtn;
    public Sprite pauseImage;
    public Sprite resumeImage;
    public Button quitBtn;

    public bool isPaused;

    public void Start()
    {
        stageText.text = "SPRING";
        resultText.text = "";
        resultTimeText.text = "";

        quitBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(false);
        resumeBtn.gameObject.SetActive(false);
    }

    public void SetChickenCount(string text)
    {
        chickenCount.text = text;
    }

    public void SetResultText(string text)
    {
        resultText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(true);
        resultText.text = text;

        replayBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(true);
    }

    public void SetResultTimeText(string text)
    {
        resultTimeText.text = "Your Record    "+ text;
    }

    public void SetStageText(string text)
    {
        stageText.gameObject.SetActive(false);
        stageText.text = text;
        stageText.gameObject.SetActive(true);
    }

    public void ChangeBtnImage()
    {
        if (isPaused)
        {
            pauseBtn.image.sprite = resumeImage;
            quitBtn.gameObject.SetActive(true);
            resumeBtn.gameObject.SetActive(true);
        }
        else
        {
            pauseBtn.image.sprite = pauseImage;
            quitBtn.gameObject.SetActive(false);
            resumeBtn.gameObject.SetActive(false);
        }
    }
}
