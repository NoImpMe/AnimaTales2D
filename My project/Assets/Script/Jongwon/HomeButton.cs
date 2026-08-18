using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리 기능을 위해 추가

public class HomeButton: MonoBehaviour
{
   public void SceneChange()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
