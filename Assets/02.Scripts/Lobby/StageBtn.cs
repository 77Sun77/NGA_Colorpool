using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageBtn : MonoBehaviour
{
    Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick_Btn);
    }

    
    void Update()
    {
        
    }

    void OnClick_Btn()
    {
        print(gameObject.name + " stage open");
    }
}
