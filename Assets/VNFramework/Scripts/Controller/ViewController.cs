using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;

public class ViewController : MonoBehaviour
{
    public GameObject configViewPrefab;
    public GameObject configView;
    public GameObject chapterViewPrefab;
    public GameObject chapterView;

    private void Awake()
    {
        GameState.UIChanged += OnUIChanged;
    }

    private void OnDestroy() {
        GameState.UIChanged -= OnUIChanged;
    }

    private void OnUIChanged(Hashtable hash)
    {
        if ((string)hash["object"] == "config")
        {
            var action = (string)hash["action"];

            if (action == "show") CreateConfigView();
            else if (action == "hide") DestoryConfigView();
        }
        else if ((string)hash["object"] == "chapter")
        {
            var action = (string)hash["action"];

            if (action == "show") CreateChapterView();
            else if (action == "hide") DestoryChapterView();
        }
    }

    public void CreateConfigView()
    {
        configView = Instantiate(configViewPrefab,transform);
    }

    public void DestoryConfigView()
    {
        Destroy(configView);
    }

    public void CreateChapterView()
    {
        chapterView = Instantiate(chapterViewPrefab,transform);
    }

    public void DestoryChapterView()
    {
        Destroy(chapterView);
    }
}