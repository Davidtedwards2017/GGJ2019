using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class ParallelActionPool : MonoBehaviour {

    private Stack<Func<IEnumerator>> actionPool = new Stack<Func<IEnumerator>>();
    //private bool Executing = false;
    private int ActionsLeft = 0;

    public void QueueAction(Func<IEnumerator> action)
    {
        if (!actionPool.Contains(action))
        {
            actionPool.Push(action);
        }
    }

    public IEnumerator Execute()
    {
        //Executing = true;
        ActionsLeft = 0;

        while(actionPool.Any())
        {
            var action = actionPool.Pop();
            StartCoroutine(Execute(action));
        }

        while (ActionsLeft > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        //Executing = false;
    }

    private IEnumerator Execute(Func<IEnumerator> action)
    {
        ActionsLeft++;
        yield return action.Invoke();
        ActionsLeft--;
    }
}
