#if UNITY_2020_2_OR_NEWER
[assembly: UnityEditor.Localization]
#else
using UnityEditor.Localization.Editor;
[assembly: Localization]
#endif
