using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEditor.Animations;

public class Startup : MonoBehaviour
{
    
    private int heroIndex;

	void Start ()
    {
        InvokeRepeating("CreateHero", 0f, 5f);
	}
    
	void Update ()
    {
	}


    void CreateHero()
    {
        var hero = new GameObject("hero-" + heroIndex++);
        hero.transform.localScale = new Vector3(2, 2);
        hero.transform.position = Vector3.zero;
        hero.AddComponent<SpriteRenderer>();
        var animator = hero.AddComponent<Animator>();
        animator.runtimeAnimatorController = new HeroAnimation().Create();
        //if(heroIndex == 1)
            hero.AddComponent<HeroController>();
    }
}
