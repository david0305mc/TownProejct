using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryCatchTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            System.Action action = () => {
                
                Debug.Log("Try1");
            };
            bool value = false;
            try
            {
                value = true;
                action();
                throw new System.Exception("test Exception");
            }
            catch
            {
                value = false;
                Debug.Log("Catch1");
                action();
            }
            finally
            {
                
            }
        }
        catch
        { 
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
