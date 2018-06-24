using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the button from the menu.
/// </summary>
public class ButtonScript : MonoBehaviour
{
    /// <summary>  Button to be managed </summary>
    public Button button;
    /// <summary>  Menu and button text </summary>
    public Text text, buttonText;
    /// <summary>  Canvas where the menu and button are rendered </summary>
    public Canvas canvas;
    private CanvasGroup menu;
    private LevelManager generator;
    private bool inGame;
    private bool start, end, gameOver;
    private DateTime startTime;

    void Start()
    {
        startTime = DateTime.Now;
        inGame = false;
        start = true;
        end = false;
        gameOver = false;
        generator = (LevelManager)(GameObject.FindGameObjectWithTag("Creator")).GetComponent<LevelManager>();
        menu = canvas.GetComponent<CanvasGroup>();
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    /// <summary>
    /// Returns the canvasgroup from the canvas where the button belongs.
    /// </summary>
    /// <returns></returns>
    public CanvasGroup getCanvasGroup()
    {
        return this.menu;
    }

    /// <summary>
    /// Click action, might start a level or move to the next one.
    /// </summary>
    public void TaskOnClick()
    {
        if (!inGame && start)
        {
            menu.alpha = 0f;
            menu.blocksRaycasts = false;
            LevelBuilder builder = generator.getBuilder();
            GameObject tmpCamera = GameObject.FindGameObjectWithTag("Temporary");
            Destroy(tmpCamera);
            generator.LevelStart();
            inGame = true;
            start = false;
            end = false;
        }
        else if (!inGame && !start && end)
        {
            menu.alpha = 0f;
            menu.blocksRaycasts = false;
            generator.NextLevel();
            inGame = true;
        } else if (!inGame && gameOver){
            menu.alpha = 0f;
            menu.blocksRaycasts = false;
            generator.DestroyLevel();
            generator.LevelStart();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PookieController>().setCash(0);
            inGame = true;
            start = false;
            end = false;
        }
    }

    /// <summary>
    /// Show the game over menu.
    /// </summary>
    public void showGameOver()
    {
        menu.alpha = 1f; //1f to show
        menu.blocksRaycasts = true; //true to enable
        text.text = "You died!!";
        buttonText.text = "Restart :(";
        inGame = false;
        gameOver = true;

        //Códico para registar a estatistica do tempo de jogo na base de dados através da API RESTFul
        TimeSpan ts = DateTime.Now - startTime;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        var jsonString = "{\"name\":\"" + PlayerPrefs.GetString("username") + "\",\"timeSpan\":\"" + ts.ToString() + "\"}";
        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());
        WWW wwwPostRequest = new WWW("http://localhost:8081/person", byteData, headers);        
    }

    /// <summary>
    /// Shows the end level menu.
    /// </summary>
    public void showEndScript()
    {
        menu.alpha = 1f;
        menu.blocksRaycasts = true;
        text.text = "Level Completed!";
        buttonText.text = "Next";
        inGame = false;
        end = true;
    }
}
