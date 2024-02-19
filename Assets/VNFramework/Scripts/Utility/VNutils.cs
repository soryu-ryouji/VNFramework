using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityObject = UnityEngine.Object;

namespace VNFramework
{
    public static class VNutils
    {
        public static Color StrToColor(string colorCode)
        {
            ColorUtility.TryParseHtmlString(colorCode, out Color color);
            return color;
        }

        
        public static IEnumerable<Transform> GetChildren(Transform transform)
        {
            return transform.Cast<Transform>().ToList();
        }

        public static GameController FindNovaController()
        {
            var go = GameObject.FindWithTag("NovaController");

            if (!go.TryGetComponent<GameController>(out var controller))
            {
                throw new Exception("VNFramework: No GameController component in GameController game object.");
            }

            return controller;
        }
    }
}