using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance;

    private void Start()
    {
        Instance = this;
    }

    public void ReadAchievement(string achievementName)
    {
        CloseUIs();
        transform.Find("Achievements").Find(achievementName).gameObject.SetActive(true);
        transform.Find("Achievements").gameObject.SetActive(true);
    }

    public void CloseUIs()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                foreach (Transform ct in t)
                {
                    if (ct.name != "Close" && ct.gameObject.activeSelf) ct.gameObject.SetActive(false);
                }
                t.gameObject.SetActive(false);
            }
        }
    }
}