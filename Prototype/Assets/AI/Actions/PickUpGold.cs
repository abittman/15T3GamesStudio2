using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class PickUpGold : RAINAction
{ 
	public PickUpGold()
	{
		actionName = "PickUpGold";
	}
	
	public override void Start(AI ai)
	{
		base.Start(ai);
		
		GameObject gold = ai.WorkingMemory.GetItem<GameObject>("gold");

		ai.WorkingMemory.SetItem<GameObject>("gold", null);
		Object.Destroy(gold);
		Debug.Log ("pick up");
		

	}
    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}