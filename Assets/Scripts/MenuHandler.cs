using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public InputField userNameInput;
    public static string userName;
    // Start is called before the first frame update
    void Start()
    {
        if(userName != null)
        {
            userNameInput.text = userName; 
        }
    }
    public void SaveName(string newName)
    {
        userName = newName;
    }

}
