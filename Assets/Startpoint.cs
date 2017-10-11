using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startpoint : MonoBehaviour
{

    public delegate void buildBubble();
    public static event buildBubble onBuildBubble;

    void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Bubble")
        {

            if (onBuildBubble != null)
            {
                onBuildBubble();
            }
            
        }
    }
}
