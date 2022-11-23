using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        if(!GlobalDJ.Instance.IsBusy() || GlobalDJ.Instance.GetCurrentIndexSong() != 0)
            GlobalDJ.Instance.PlaySong(0, true);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
