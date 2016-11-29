using UnityEngine;
using System.Collections;

public class Error : MonoBehaviour {

	public static void ShowError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogError(message);
        if(UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
