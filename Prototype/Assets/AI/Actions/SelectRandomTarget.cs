using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Navigation.Graph;
using RAIN.Navigation;

[RAINAction]
public class SelectRandomTarget : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
		var loc = Vector3.zero;
		List<RAINNavigationGraph> found = new List<RAINNavigationGraph>();
		do
		{
			loc = ai.Kinematic.Position;
			loc.x += Random.Range(-8f, 8f);
			loc.z += Random.Range(-8f, 8f);
			found = NavigationManager.Instance.GraphsForPoints(
				ai.Kinematic.Position,
				loc, 
				ai.Motor.StepUpHeight,
				NavigationManager.GraphType.Navmesh,
				((BasicNavigator)ai.Navigator).GraphTags);
		} 
		while ( Vector3.Distance(loc, ai.Kinematic.Position) < 2f 
		       || found.Count == 0);
		ai.WorkingMemory.SetItem<Vector3>("TargetPoint", loc);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}