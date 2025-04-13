using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        CameraFade.fadeInstance.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton() 
    {
        CameraFade.fadeInstance.FadeOut();
        Invoke("GameStart", 2f);
    }

    void GameStart()
    {
        SceneManager.LoadScene("LeeScene");
    }
}
