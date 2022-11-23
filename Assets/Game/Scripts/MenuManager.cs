using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        if(!GlobalDJ.Instance.IsBusy())
            GlobalDJ.Instance.PlaySong(0, true);
    }

}
