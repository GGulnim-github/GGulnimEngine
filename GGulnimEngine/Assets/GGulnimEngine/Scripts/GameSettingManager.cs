using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingManager : Singleton<GameSettingManager>
{
    public int TargetFrameRate = 60;

#if !UNITY_EDITOR
    private void Awake()
    {

        Application.targetFrameRate = TargetFrameRate;
    }
#endif

    public void SetTargetFrame(int value)
    {
        Application.targetFrameRate = value;
    }
}
