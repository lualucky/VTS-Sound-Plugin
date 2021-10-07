using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public float ClickTime;
    public GameObject Menu;

    bool click;

    void Update()
    {
        if(!Menu.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            if (click)
            {
                ShowMenu();
                StartCoroutine(ClickTimeout());
            }
            else
                click = true;
        }
    }

    IEnumerator ClickTimeout()
    {
        yield return new WaitForSeconds(ClickTime);
        click = false;
    }

    public void ShowMenu() {
        Menu.SetActive(true);
    }

    public void HideMenu()
    {
        Menu.SetActive(false);
    }
}
