using UnityEngine;

public class UI : MonoBehaviour
{
    public bool IsOpen { get => Cursor.visible; }

    public static UI Instance;

    private void Start()
    {
        Instance = this;
        LockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ReadAchievement(string achievementName)
    {
        CloseUIs();
        GameObject.Find("UI").transform.Find("Achievements").Find(achievementName).gameObject.SetActive(true);
        GameObject.Find("UI").transform.Find("Achievements").gameObject.SetActive(true);
        UnlockMouse();
    }

    public void CloseUIs()
    {
        LockMouse();
        Transform t = GameObject.Find("UI").transform.Find("Achievements");
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