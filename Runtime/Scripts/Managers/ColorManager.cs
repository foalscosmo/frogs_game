using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField] private List<string> skeletonContainer = new();

        [SerializeField] private List<SkeletonMecanim> frogsSkeletons = new();

        [SerializeField] private List<Sprite> floatsContainer = new();
        [SerializeField] private List<SpriteRenderer> chosenFloats = new();
        private void Awake()
        {
            SetColorToObjects();
        }

        public void SetColorToObjects()
        {
            ShuffleTogether(skeletonContainer, floatsContainer);
            Shuffle(frogsSkeletons);

            var colorToLayerMap = new Dictionary<string, int>();

            for (var i = 0; i < frogsSkeletons.Count; i++)
            {       
                chosenFloats[i].sprite = floatsContainer[i];
                
                frogsSkeletons[i].Skeleton.SetSkin(skeletonContainer[i]);
                frogsSkeletons[i].Skeleton.SetSlotsToSetupPose();

                if (!colorToLayerMap.ContainsKey(skeletonContainer[i]))
                {
                    var nextLayerIndex = colorToLayerMap.Count + 6; 
                    colorToLayerMap[skeletonContainer[i]] = nextLayerIndex;
                }

                frogsSkeletons[i].gameObject.layer = colorToLayerMap[skeletonContainer[i]];
                chosenFloats[i].gameObject.layer = colorToLayerMap[skeletonContainer[i]];
            }
        }

        private void ShuffleTogether(IList<string> list1, IList<Sprite> list2)
        {
            int n = Mathf.Min(list1.Count, list2.Count); 

            for (var i = n - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list1[i], list1[j]) = (list1[j], list1[i]);
                (list2[i], list2[j]) = (list2[j], list2[i]);
            }
        }
        private static void Shuffle<T>(IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
