using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EasyMobile;

public class GPS_init : MonoBehaviour
{
    public static GPS_init Instance { get; set; }
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }


    void Start()
    {
        GameServices.Init();
    }
    
    public void UnlockAchievement(int index)
    {
        if (GameServices.IsInitialized())
        {
            if(index == 9)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Baby_Steps);
            }
            else if (index == 3)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Beginner);
            }
            else if(index == 4)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Advanced);
            }
            else if (index == 5)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Master);
            }
            else if(index == 6)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Persistent_1);
            }
            else if (index == 7)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Persistent_2);
            }
            else if(index == 8)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Persistent_3);
            }
            else if (index == 1)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Nerd);
            }
            else if (index == 0)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Sugar_Rush);
            }
            else if (index == 2)
            {
                GameServices.UnlockAchievement(EM_GameServicesConstants.Achievement_Zombie);
            }
        }
    }

    public void ShowAchievements()
    {
        if (GameServices.IsInitialized())
        {
            GameServices.ShowAchievementsUI();
        }
        else
        {
#if UNITY_ANDROID
            GameServices.Init();
#endif
        }
    }

}
