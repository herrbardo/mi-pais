using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] public NaturalResource ResourceInfo;
    
    [Header("Components")]
    [SerializeField] Image Logo;

    private void Start()
    {
        Logo.sprite = ResourceInfo.Logo;
    }
}
