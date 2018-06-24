using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{

    public string MainSceneName;

    public InputField UsernameText;


    void Start()
    {
        string username = PlayerPrefs.GetString("username", "guest");
        print(username);
        UsernameText.text = username;
    }


    public void PlayButtonClick ()
    {
        PlayerPrefs.SetString("username", UsernameText.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene(MainSceneName);
    }


}
