using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTest : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly int totalFrame = 2;
    private int count;
    private void Awake()
    {
        animator.speed = 0f;
    }

    public void OnClickPauseTest()
    {
        if (count >= totalFrame)
        {
            count = 0;
        }
        else
        {
            count++;
        }
        float nomalizeTime = (count / (float)totalFrame);
        animator.Play("Pause0", 0, nomalizeTime);

    }


}
