using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.Animations;

public sealed class HeroAnimation : AbstractEntityAnimation
{
    public HeroAnimation() : base(CreateHeroAnimation, "hero/0")
    {
    }

    private static void CreateHeroAnimation(AnimatorController controller)
    {
        // TODO: attack xx
    }

    public AnimatorController Create()
    {
        return base.Create("hero/0");
    }
}
