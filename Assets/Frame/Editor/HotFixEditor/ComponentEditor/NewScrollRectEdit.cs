﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{

    [CustomEditor(typeof(NewScrollRect), true)]
    [CanEditMultipleObjects]
    public class NewScollRectEdit: UnityEditor.UI.ScrollRectEditor
    {
        //NewScrollRect mText;
        protected override void OnEnable()
        {
            base.OnEnable();
           
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.Update();
            //mText = (NewScrollRect)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Left"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Right"));
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}










































