using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test01 : Singleton<Test01>
{
    private void Start()
    {
        Invoke(nameof(Test), 2f);
    }

    public void Test()
    {
        SceneManager.LoadScene("Test");
    }
}
