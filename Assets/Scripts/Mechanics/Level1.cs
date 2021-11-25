using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
  
    public void Awake()
    {
        
    }
    private void Start()
    {
        GameManager.Instance.Level1Start();
    }
}
