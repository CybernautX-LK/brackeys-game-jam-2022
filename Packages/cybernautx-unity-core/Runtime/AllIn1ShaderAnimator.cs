using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class AllIn1ShaderAnimator : MonoBehaviour
{
    public enum PropertyType { Float, Color }

    public enum SequenceConnection { Join, Append, Prepend }
    public enum TargetType { Image, SpriteRenderer }

    [System.Serializable]
    public class Property
    {
        public SequenceConnection sequenceConnection = SequenceConnection.Join;
        public PropertyType type = PropertyType.Float;
        public string materialProperty = string.Empty;

        [ShowIf("type", PropertyType.Float)]
        public float targetValue = 1.0f;

        [ShowIf("type", PropertyType.Color)]
        public Color targetColor = Color.white;

        public float duration = 1.0f;

        public Ease ease = Ease.Unset;
    }

    [System.Serializable]
    public class Animation
    {
        public bool playOnEnable = false;
        
        public string name = string.Empty;
        public float delay = 0.0f;
        public int loops = 0;
        public LoopType loopType = LoopType.Restart;
        public List<Property> properties = new List<Property>();
        public DG.Tweening.Sequence sequence = null;
    }

    [SerializeField]
    private TargetType targetType = TargetType.Image;    

    public Animation[] animations = null;

    private Material targetMaterial = null;

    private Material originalMaterial = null;

    private void Awake()
    {
        GetTargetMaterial();

        originalMaterial = targetMaterial;
    }

    private void Start()
    {
        foreach (Animation animation in animations)
        {
            if (animation.playOnEnable)
                PlayAnimation(animation.name);
        }
    }

    private void OnDestroy()
    {
        targetMaterial = originalMaterial;
    }

    [Button]
    public void PlayAnimation(string name)
    {
        Animation animation = GetAnimationByName(name);
        
        if (animation != null)
        {
            if (DOTween.IsTweening(targetMaterial))
                DOTween.Kill(targetMaterial);

            animation.sequence = DOTween.Sequence();
            Tween tween = null;

            foreach (Property property in animation.properties)
            {
                switch (property.type)
                {
                    case PropertyType.Float:

                        tween = targetMaterial.DOFloat(property.targetValue, property.materialProperty, property.duration).SetEase(property.ease);
                        

                        break;
                    case PropertyType.Color:

                        tween = targetMaterial.DOColor(property.targetColor, property.materialProperty, property.duration).SetEase(property.ease);

                        break;
                    default:
                        break;
                }

                switch (property.sequenceConnection)
                {
                    case SequenceConnection.Join:
                        animation.sequence.Join(tween);
                        break;
                    case SequenceConnection.Append:
                        animation.sequence.Append(tween);
                        break;
                    case SequenceConnection.Prepend:
                        animation.sequence.Prepend(tween);
                        break;
                    default:
                        break;
                }
                              
            }

            animation.sequence.PrependInterval(animation.delay);
            animation.sequence.SetLoops(animation.loops, animation.loopType);
        }
    }

    public void StopAnimation(string animationName)
    {
        Animation animation = GetAnimationByName(name);

        if (animation != null && animation.sequence != null)
        {
            animation.sequence.Rewind();
            animation.sequence.Pause();
        }
    }

    public Property GetAnimationPropertyByName(string animationName, string propertyName)
    {
        foreach (Animation animation in animations)
        {
            if (animation.name == animationName)
            {
                foreach (Property property in animation.properties)
                {
                    if (property.materialProperty == propertyName)
                        return property;
                }

                Debug.LogWarning("[$this.name]: There is no property" + propertyName + " in " + animationName);
                return null;
            }
        }

        Debug.LogWarning("[$this.name]: There is no animation " + animationName);
        return null;
    }

    public Animation GetAnimationByName(string name)
    {
        foreach (Animation animation in animations)
        {
            if (animation.name == name)
                return animation;
        }

        return null;
    }

    private void GetTargetMaterial()
    {
        switch (targetType)
        {
            case TargetType.Image:

                targetMaterial = GetComponent<Image>().material;

                break;
            case TargetType.SpriteRenderer:

                targetMaterial = GetComponent<SpriteRenderer>().material;

                break;
            default:
                break;
        }
    }
}
