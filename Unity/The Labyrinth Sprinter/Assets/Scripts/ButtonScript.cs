using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public enum Difficulty
    {
        Small = 3,
        Medium = 5,
        Large = 7
    }

    public static bool pressed = false;
    public static Difficulty difficulty = Difficulty.Small;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Button pressed");
        pressed = true;
        string name = GetComponent<Transform>().name;
        if (name == "button1")
        {
            difficulty = Difficulty.Small;
        }
        else if (name == "button2")
        {
            difficulty = Difficulty.Medium;
        }
        else if (name == "button3")
        {
            difficulty = Difficulty.Large;
        }
        else
        {
            throw new System.Exception("Button: \"" + name + "\" is unknown");
        }
    }
}
