using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.SceneManagement;
using System;

public class VeryMainScript : MonoBehaviour {
    public static string levelData;

    private string[] names, maps;
    private int selectedScene = -1;
    private Text text;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    void Start () {
        WebClient client = new WebClient();
        client.DownloadStringCompleted += ClientCalbeack;
        client.DownloadStringAsync(new Uri("http://localhost:4000/game"));
        text = GameObject.Find("MainButtonText").GetComponent<Text>();
        Cursor.visible = true;
    }

    private void ClientCalbeack(object sender, DownloadStringCompletedEventArgs e)
    {
        string data = "";
        try
        {
            data = e.Result;
        }
        catch (Exception)
        {

        }

        List<string> bufer = new List<string>();

        int i1 = 0, i2;
        while (i1 != -1)
        {
            i2 = data.IndexOf("~", i1);

            bufer.Add(data.Substring(i1, i2 - i1));

            i1 = ((i2 + 1) == data.Length) ? -1 : i2 + 1;
        }

        string[] array = bufer.ToArray();
        names = new string[array.Length / 2];
        maps = new string[array.Length / 2];

        for (int key = 0, i = 0; key < array.Length; i++)
        {
            names[i] = array[key];
            key++;
            maps[i] = array[key];
            key++;
        }
    }

    public void ChangeScene (bool isForwrd)
    {
        if (isForwrd && selectedScene == maps.Length - 1)
        {
            selectedScene = -1;
        }
        else
        {
            if (!isForwrd && selectedScene == -1)
            {
                selectedScene = maps.Length - 1;
            }
            else
            {
                selectedScene += isForwrd ? 1 : -1;
            }
        }

        if (selectedScene == -1)
        {
            text.text = "Сценарий";
        }
        else
        {
            text.text = names[selectedScene];
            VeryMainScript.levelData = maps[selectedScene];
        }
    }

    public void StartLevel ()
    {
        SceneManager.LoadScene((selectedScene == -1) ? 1 : 2);
    }
}
