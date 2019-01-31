using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{

	public void MainMenuBtn()
	{

	}
	public void GameOverBtn()
	{
#if UNITY_EDITOR
		// 실행상태를 해제합니다.
		UnityEditor.EditorApplication.isPlaying = false;

#else   //유니티 에디터가 아니라면
        //프로그램을 종료할때 사용하는 메소드
        Application.Quit();
#endif
	}
}
