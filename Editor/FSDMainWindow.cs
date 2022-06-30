using System;
using UnityEditor;
using UnityEngine;

namespace Autch.FSDresser
{
    public class FSDMainWindow : EditorWindow
    {
        [MenuItem("Window/Autch/FSDresser")]
        public static void OpenWindow()
        {
            var w = GetWindow<FSDMainWindow>();
            w.titleContent = new GUIContent("FS Dresser");
            w.Show();
        }

        private bool _showObjectList;
        private int _numObjectsToWear;
        private GameObject _avatarObject;
        private GameObject[] _objectsToWear = Array.Empty<GameObject>();

        private Vector2 _scrollPosition = Vector2.zero;

        private bool _lastCheck;

        public void OnGUI()
        {
            EditorGUILayout.Space();
            var s = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                normal =
                {
                    textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black
                }
            };
            GUILayout.Label(LocaleResources.Title, s);
            EditorGUILayout.Space();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            _avatarObject =
                EditorGUILayout.ObjectField(LocaleResources.AvatarToWear, _avatarObject, typeof(GameObject), true) as GameObject;

            _numObjectsToWear = EditorGUILayout.IntField(LocaleResources.NumOfItemsToWear, _numObjectsToWear);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PrefixLabel(LocaleResources.AddOrRemoveItemInList);
                if (GUILayout.Button("+") && _numObjectsToWear < 1000)
                {
                    _numObjectsToWear++;
                    if (_numObjectsToWear == 1)
                        _showObjectList = true;
                }

                if (GUILayout.Button("-") && _numObjectsToWear > 0)
                {
                    _numObjectsToWear--;
                }
            }

            if (_objectsToWear.Length != _numObjectsToWear)
            {
                Array.Resize(ref _objectsToWear, _numObjectsToWear);
            }

            _showObjectList = EditorGUILayout.Foldout(_showObjectList, string.Format(LocaleResources.ListOfItemsToWear, _numObjectsToWear), true);
            if (_showObjectList)
            {
                for (var i = 0; i < _numObjectsToWear; i++)
                {
                    _objectsToWear[i] =
                        EditorGUILayout.ObjectField(string.Format(LocaleResources.ItemToWear, i + 1), _objectsToWear[i], typeof(GameObject), true) as
                            GameObject;
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(LocaleResources.Verify))
                {
                    var ret = _lastCheck = FsdComposer.CheckToGo(_avatarObject, _objectsToWear);
                    Debug.Log(LocaleResources.VerificationResult + ": " + (ret ? "OK" : "NG"));
                    Debug.developerConsoleVisible = true;
                }

                GUI.enabled = _lastCheck;
                if (GUILayout.Button(LocaleResources.DoIt))
                {
                    try
                    {
                        var obj = FsdComposer.DoCompose(_avatarObject, _objectsToWear);
                        EditorUtility.DisplayDialog("FSDresser", LocaleResources.Success, "OK");
                        Selection.activeGameObject = obj;
                    }
                    catch (FSDException fe)
                    {
                        Debug.LogException(fe, fe.What);
                        EditorUtility.DisplayDialog("FSDresser", LocaleResources.FailedToCompileAvatar + "\n\n" + fe.Message, LocaleResources.Abort);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        EditorUtility.DisplayDialog("FSDresser", LocaleResources.FailedToCompileAvatar + "\n\b" + e.Message, LocaleResources.Abort);
                    }
                }

                GUI.enabled = true;
            }

            EditorGUILayout.EndScrollView();
        }

        public FSDMainWindow()
        {
            FsdComposer = new FSDComposer();
        }

        public FSDComposer FsdComposer { get; }
    }
}