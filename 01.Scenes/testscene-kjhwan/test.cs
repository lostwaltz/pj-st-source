using System;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] Transform baseTf;
    [SerializeField] Transform attackerTf;
    [SerializeField] Transform[] directions;

    [ContextMenu("Test Call")]
    public void TestCall()
    {
        Vector3 attackingDir = baseTf.position - attackerTf.position;
        Vector3 defensingDir = attackerTf.position - baseTf.position;
        
        Debug.Log($"attack dir : {attackingDir.normalized}");
        Debug.Log($"defense dir : {defensingDir.normalized}");
        
        Debug.DrawRay(attackerTf.position, attackingDir, Color.red, 5f);
        Debug.DrawRay(baseTf.position, defensingDir, Color.green, 5f);
        
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 blockingDir = directions[i].transform.position - baseTf.position;
            float dot = Vector3.Dot(defensingDir.normalized, blockingDir.normalized);
            Debug.Log($"{directions[i].name} : {blockingDir.normalized}\n{dot}");
        }
    }

    public Action Action1;
    public Action Action2;

    private void Start()
    {
        Action1 = TestAction1;
        Action2 = TestAction2;
    }

    public void TestAction1()
    {
        Debug.Log("TestAction 1 Called");
    }
    public void TestAction2()
    {
        Debug.Log("TestAction 2 Called");
    }

    [ContextMenu("AddActionTest")]
    public void AddActionTest()
    {
        Action1 += Action2;
    }

    [ContextMenu("CallAddedAction")]
    public void CallAddedAction()
    {
        Action1?.Invoke();
    }
}
