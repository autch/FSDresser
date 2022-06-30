using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Autch.FSDresser
{
    public class FSDException : Exception
    {
        public FSDException(string message, GameObject what): base(message)
        {
            What = what;
        }

        public GameObject What{ get; }
    }

    public class FSDComposer
    {
        private static readonly Vector3 OffsetVector = new Vector3(2.0f, 0.0f, 0.0f);

        public bool CheckToGo(GameObject avatarObject, IEnumerable<GameObject> objectsToWear)
        {
            if (!avatarObject.TryGetComponent(out Animator animatorComponent))
            {
                Debug.LogError(string.Format(LocaleResources.AvatarDoesNotHaveAnimatorComponent, avatarObject.name), avatarObject);
                return false;
            }

            var hips = animatorComponent.GetBoneTransform(HumanBodyBones.Hips);
            if (hips == null)
            {
                Debug.LogError(string.Format(LocaleResources.AvatarDoesNotHaveHipsBoneDefined, avatarObject.name), avatarObject);
                return false;
            }

            foreach (var item in objectsToWear)
            {
                // TODO: 
            }

            return true;
        }

        public GameObject DoCompose(GameObject avatarObject, IEnumerable<GameObject> objectsToWear)
        {
            var dupedAvatar = Object.Instantiate((Object)avatarObject, avatarObject.transform.position + OffsetVector,
                Quaternion.identity) as GameObject;

            if (dupedAvatar == null)
            {
                throw new FSDException(string.Format(LocaleResources.FailedToDuplicateAvatar, avatarObject.name), avatarObject);
            }

            if (PrefabUtility.IsAnyPrefabInstanceRoot(dupedAvatar))
                PrefabUtility.UnpackPrefabInstance(dupedAvatar, PrefabUnpackMode.OutermostRoot,
                    InteractionMode.AutomatedAction);

            dupedAvatar.name += "_Composed";

            var dupedObjects = new List<GameObject>();
            foreach (var part in objectsToWear)
            {
                var dupedItem = Object.Instantiate((Object)part, Vector3.zero, Quaternion.identity) as GameObject;
                if (dupedItem == null)
                {
                    throw new FSDException(string.Format(LocaleResources.FailedToDuplicateItem, part.name), part);
                }

                if (PrefabUtility.IsAnyPrefabInstanceRoot(dupedItem))
                    PrefabUtility.UnpackPrefabInstance(dupedItem, PrefabUnpackMode.OutermostRoot,
                        InteractionMode.AutomatedAction);

                dupedItem.transform.SetParent(dupedAvatar.transform);
                dupedItem.transform.localPosition = Vector3.zero;
                dupedObjects.Add(dupedItem);
            }

            var avatarHips = GetAvatarsRootBoneTransform(dupedAvatar);

            foreach (var dupedItem in dupedObjects)
            {
                var hips = dupedItem.transform.Find("Armature/Hips");
                if (hips == null)
                    continue;
                Transform transform;
                (transform = hips.transform).SetParent(avatarHips);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;

                MoveChildrenToAvatar(avatarHips, hips);
            }

            return dupedAvatar;
        }

        private static Transform GetAvatarsRootBoneTransform(GameObject obj)
        {
            if (!obj.TryGetComponent(out Animator animator))
                throw new FSDException(string.Format(LocaleResources.AvatarDoesNotHaveAnimatorComponent, obj.name), obj);
            var hips = animator.GetBoneTransform(HumanBodyBones.Hips);
            if (hips == null)
                throw new FSDException(string.Format(LocaleResources.AvatarDoesNotHaveHipsBoneDefined, obj.name), obj);
            return hips;
        }

        private static void MoveChildrenToAvatar(Transform avatarHips, Transform hips)
        {
            var children = new List<Transform>();
            var cc = hips.childCount;
            for (var i = 0; i < cc; i++)
            {
                var o = hips.GetChild(i);
                if (o.name.EndsWith("_end"))
                    continue;
                children.Add(o);
            }

            foreach (var o in children)
            {
                var ao = avatarHips.Find(o.name);
                if (ao == null) continue;
                o.SetParent(ao);
                MoveChildrenToAvatar(ao, o);
            }
        }
    }
}