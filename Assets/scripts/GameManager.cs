using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string filename = "\\world.txt";
    [HideInInspector] public GameObject player;

    public static GameManager GetInstance() { return _instance; }
    private static GameManager _instance;

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }

        transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = File.Exists(Application.dataPath + filename);
        DontDestroyOnLoad(gameObject);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Save(string path)
    {
        List<string> blocks = new List<string>();
        foreach (Transform block in transform)
        {
            string tmp = "";
            tmp += block.GetComponent<ItemID>().id + ";";
            tmp += block.position.x + ";";
            tmp += block.position.y + ";";
            tmp += block.position.z;
            blocks.Add(tmp);
        }
        File.WriteAllLines(path, blocks.ToArray());
    }

    public void Load()
    {
        Load();
    }

    public void Create()
    {
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            bool current = GetComponent<Canvas>().enabled;
            GetComponent<Canvas>().enabled = !current;
            
            player.GetComponent<UnityStandardAssets
                .Characters.FirstPerson
                .FirstPersonController>().enabled =
                current;

            if (!current) Cursor.lockState =
                    CursorLockMode.None;
            Cursor.visible = !current;

            Time.timeScale = current ? 1 : 0;
        }
    }
    void OnLevelWasLoaded(int level)
    {
        if(level != 0)
        {
            GetComponent<Canvas>().enabled = false;
        }
    }
}
