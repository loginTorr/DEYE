using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackToStart : MonoBehaviour
{
    public void BackToStartButton()
    {
        CameraFade.fadeInstance.FadeOut();
        Invoke("GoBackToStart", 2f);
    }

    void GoBackToStart()
    {
        SceneManager.LoadScene("StartScene");
    }
}