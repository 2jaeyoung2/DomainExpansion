#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
#endif
using UnityEngine;

public class ClearLog : MonoBehaviour
{
    private static Action clearConsoleDelegate;

#if UNITY_EDITOR
    private void Awake()
    {
        if (clearConsoleDelegate == null)
        {
            // UnityEditor.LogEntries 타입 얻기
            var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            // Clear 메서드 정보 가져오기
            var clearMethod = logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            // 델리게이트 캐싱
            clearConsoleDelegate = (Action)Delegate.CreateDelegate(typeof(Action), clearMethod);
        }
    }
#endif

    public void ClearEditorConsole()
    {
#if UNITY_EDITOR
        clearConsoleDelegate?.Invoke();
#else
        Debug.LogWarning("콘솔 지우기는 에디터에서만 동작합니다.");
#endif
    }
}
