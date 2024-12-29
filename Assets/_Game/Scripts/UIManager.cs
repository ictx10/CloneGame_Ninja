using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    /*    public static UIManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindAnyObjectByType<UIManager>();    
                }
                return instance;
            }
        }*/

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] Text coiText;
    public void SetCoin(int coin)
    {
        coiText.text = coin.ToString();

    }
}
