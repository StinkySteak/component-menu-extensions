using System;
using UnityEditor;
using UnityEngine;

namespace StinkySteak.ComponentMenu
{
    public static class ComponentMenuExtensions
    {
        private static string _cutComponentData = string.Empty;
        private static Type _cutComponentType = null;

        [MenuItem("CONTEXT/Component/Cut Component", priority = 111)]
        private static void CutComponent(MenuCommand command)
        {
            Component component = (Component)command.context;
            _cutComponentData = EditorJsonUtility.ToJson(component);
            _cutComponentType = component.GetType();
            Undo.DestroyObjectImmediate(component);
        }

        [MenuItem("CONTEXT/Component/Cut Component", validate = true, priority = 111)]
        private static bool ValidateCutComponent(MenuCommand command)
        {
            Component component = (Component)command.context;
            return component.GetType() != typeof(Transform);
        }

        [MenuItem("CONTEXT/Component/Paste Cut Component", priority = 111)]
        private static void PasteCutComponent(MenuCommand command)
        {
            Component component = (Component)command.context;
            var addedComponent = Undo.AddComponent(component.gameObject, _cutComponentType);
            EditorJsonUtility.FromJsonOverwrite(_cutComponentData, addedComponent);
            _cutComponentData = string.Empty;
            _cutComponentType = null;
        }

        [MenuItem("CONTEXT/Component/Paste Cut Component", validate = true, priority = 111)]
        private static bool ValidatePasteCutComponent()
        {
            return !string.IsNullOrEmpty(_cutComponentData) && _cutComponentType != null;
        }
    }
}