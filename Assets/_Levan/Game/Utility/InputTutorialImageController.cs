using System.Collections;
using System.Collections.Generic;
using LevanPangInterview;
using LevanPangInterview.Models;
using UnityEngine;

public class InputTutorialImageController : MonoBehaviour
{
    public int PlayerNumber;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            this.gameObject.SetActive(false);
            return;

        }

        if (Link.GetData<GameplayModel>().PlayerCount >= this.PlayerNumber)
            this.gameObject.SetActive(true);
        else 
            this.gameObject.SetActive(false);
    }

}
