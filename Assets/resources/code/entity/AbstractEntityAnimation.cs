using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Animations;
using UnityEngine;

public enum EntityDirection : int
{
    SOUTH = 0,
    WEST = 1,
    NORTH = 2,
    EAST = 3,
}

public abstract class AbstractEntityAnimation : AbstractAnimationCache
{
    private const string SPRITES_ANIM_PATH = "sprites/";
    private const string PARAM_DIRECTION = "direction";

    protected AbstractEntityAnimation(SpecificAnimatorCreator specificAnimatorCreator, string path)
        : base(controller =>
        {
            CreateEntityAnimation(path, controller);
            specificAnimatorCreator(controller);
        })
    {
    }

    private static void CreateEntityAnimation(string path, AnimatorController controller)
    {
        controller.AddParameter(PARAM_DIRECTION, AnimatorControllerParameterType.Int);

        var baseLayer = controller.layers[0];
        
        var states = new[]
        {
            baseLayer.stateMachine.AddState("walk_south"),
            baseLayer.stateMachine.AddState("walk_west"),
            baseLayer.stateMachine.AddState("walk_north"),
            baseLayer.stateMachine.AddState("walk_east")
        };

        // walk south
        baseLayer.stateMachine.AddEntryTransition(states[0]);

        // add transitions between each state
        for (var i = 0; i < states.Length; i++)
        {
            var stateA = states[i];
            stateA.motion = Resources.Load<AnimationClip>(
                Path.Combine(
                    SPRITES_ANIM_PATH, 
                    string.Format("{0}_walk_{1}" , path, i)));
            for (var j = 0; j < states.Length; j++)
            {
                if (i == j)
                    continue;

                var stateB = states[j];
                var transition = stateA.AddTransition(stateB, false);
                transition.AddCondition(AnimatorConditionMode.Equals, j, PARAM_DIRECTION);
                transition.duration = 0;
                transition.exitTime = 0;
            }
        }
    }
}
