using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public abstract class AbstractAnimationCache : AbstractCache<string, AnimatorController>
{
    private const string ANIM_PATH = "dynamic/animation/";
    private const string ANIM_FULLPATH = "Assets/" + ANIM_PATH;

    public delegate void SpecificAnimatorCreator(AnimatorController controller);

    private static readonly Checker CheckDynamicAnim
        = fileName => File.Exists(Path.Combine(Application.dataPath, Path.Combine(ANIM_PATH, fileName + ".controller")));

    private static Creator CompileDynamicAnim(SpecificAnimatorCreator specificAnimatorCreator)
    {
        return fileName =>
        {
            var fullPath = Path.Combine(ANIM_FULLPATH, fileName + ".controller");
            Directory.CreateDirectory(fullPath);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(fullPath);
            // TODO : common code
            specificAnimatorCreator(controller);
            return null;
        };
    }

    protected AbstractAnimationCache(SpecificAnimatorCreator specificAnimatorCreator)
        : base(CheckDynamicAnim, CompileDynamicAnim(specificAnimatorCreator))
    {
    }

    public virtual AnimatorController Create(string animation)
    {
        if (!m_checker(animation))
            m_creator(animation);
        return AssetDatabase.LoadAssetAtPath<AnimatorController>(Path.Combine(ANIM_FULLPATH, animation) + ".controller");
    }
}

public abstract class AbstractCache<TKey, TValue>
{
    public delegate bool Checker(TKey key);

    public delegate TValue Creator(TKey key);

    protected readonly Checker m_checker;
    protected readonly Creator m_creator;

    protected AbstractCache(Checker checker, Creator creator)
    {
        m_checker = checker;
        m_creator = creator;
    }
}

public abstract class AbstractResourceCache<T> where T : UnityEngine.Object
{
    private readonly Dictionary<string, T> m_cache;

    protected AbstractResourceCache()
    {
        m_cache = new Dictionary<string, T>();
    }

    public T Load(string path)
    {
        if (!m_cache.ContainsKey(path))
            m_cache[path] = AssetDatabase.LoadAssetAtPath<T>(path);
        return m_cache[path];
    }

    protected abstract T Create(string path);
}
