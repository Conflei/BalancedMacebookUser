using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{

    public InputField username;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScreen()
    {
        PlayerPrefs.SetString("username", username.text);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
