using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LandscapeGenerator : MonoBehaviour
{
    [SerializeField] string filename = "\\world.txt";
    public GameObject[] prefabs;
    public GameObject bedrock;
    public int size;
    public int maxHeight;
    void Awake() {
        if (File.Exists(Application.dataPath + filename))
        {
            Load(Application.dataPath + filename);
        }
        else
        {
            Create();
        }
    }
    void OnDestroy()
    {
        Save(Application.dataPath + filename);
    }
    void Save(string path)
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
    void Load(string path)
    {
        string[] blocks = File.ReadAllLines(path);
        foreach (string String in blocks)
        {
            string[] tmp = String.Split(';');
            if (int.Parse(tmp[0]) != 5)
            {
                GameObject.Instantiate(prefabs[int.Parse(tmp[0])], new Vector3(int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3])), Quaternion.identity, transform);
            }
            else
            {
                GameObject.Instantiate(bedrock, new Vector3(int.Parse(tmp[1]), int.Parse(tmp[2]), int.Parse(tmp[3])), Quaternion.identity, transform);
            }
        }
    }
    void Create()
    {
        int seed = Random.Range(0, 16);
        print(seed);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int y = (int)(maxHeight * Mathf.PerlinNoise(seed + i / 32f, seed + j / 32f));
                while (y >= 0)
                {
                    GameObject.Instantiate(
                        prefabs[(int)(y * prefabs.Length * (1f / maxHeight))],
                        new Vector3(i, y, j),
                        Quaternion.identity,
                        transform
                    );
                    y--;
                }
                GameObject.Instantiate(
                        bedrock,
                        new Vector3(i, -1, j),
                        Quaternion.identity,
                        transform
                    );
            }
        }
    }
}
