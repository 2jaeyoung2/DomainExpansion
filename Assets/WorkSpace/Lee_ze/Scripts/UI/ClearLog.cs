using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ClearLog : MonoBehaviour
{
    private static Action clearConsoleDelegate;

#if UNITY_EDITOR
    private void Awake()
    {
        if (clearConsoleDelegate == null)
        {
            // UnityEditor.LogEntries Ÿ�� ���
            var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            // Clear �޼��� ���� ��������
            var clearMethod = logEntries?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            // ��������Ʈ ĳ��
            clearConsoleDelegate = (Action)Delegate.CreateDelegate(typeof(Action), clearMethod);
        }
    }
#endif

    public void ClearEditorConsole()
    {
#if UNITY_EDITOR
        clearConsoleDelegate?.Invoke();
#else
        Debug.LogWarning("�ܼ� ������ �����Ϳ����� �����մϴ�.");
#endif
    }
}
