using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public abstract class DictionaryDrawer<TK, TV> : PropertyDrawer {
    
    private SerializableDictionary<TK, TV> _Dictionary;
    private bool _Foldout;
    private const float kButtonWidth = 18f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        CheckInitialize(property, label);
        if (!_Foldout || _Dictionary.Count == 0) return 17f;
        float height = 17f;
        Type type = typeof(TV);
        if (type == typeof(DInt)) {
            foreach (var dInt in _Dictionary.Values) height += DIntDrawer.GetPropertyHeight(dInt as DInt);
        } else if (type == typeof(DLong)) {
            foreach (var dLong in _Dictionary.Values) height += DLongDrawer.GetPropertyHeight(dLong as DLong);
        } else if (type == typeof(DFloat)) {
            foreach (var dFloat in _Dictionary.Values) height += DFloatDrawer.GetPropertyHeight(dFloat as DFloat);
        } else height = _Dictionary.Count * 17f;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        CheckInitialize(property, label);
        
        EditorGUI.DrawRect(position, new Color(.85f, .85f, .85f));

        position.height = 17f;

        var foldoutRect = position;
        foldoutRect.width -= 2 * kButtonWidth;
        EditorGUI.BeginChangeCheck();
        _Foldout = EditorGUI.Foldout(foldoutRect, _Foldout, label, true);
        if (EditorGUI.EndChangeCheck()) {
            EditorPrefs.SetBool(label.text, _Foldout);
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        var buttonRect = position;
        buttonRect.x = position.width - kButtonWidth + position.x;
        buttonRect.width = kButtonWidth + 2;

        if (GUI.Button(buttonRect, new GUIContent("+", "Add item"), EditorStyles.miniButton)) AddNewItem();
        buttonRect.x -= kButtonWidth;

        if (GUI.Button(buttonRect, new GUIContent("X", "Clear dictionary"), EditorStyles.miniButtonRight)) ClearDictionary();
        if (!_Foldout) return;

        float extra = 19f;
 
        foreach (var item in _Dictionary) {
            var key = item.Key;
            var value = item.Value;

            position.y += extra;
 
            var keyRect = position;
            keyRect.width /= 3f;
            keyRect.width -= 4f;
            EditorGUI.BeginChangeCheck();
            var newKey = DoField(keyRect, typeof(TK), key, property);
            if (EditorGUI.EndChangeCheck()) {
                try {
                    _Dictionary.Remove(key);
                    _Dictionary.Add(newKey, value);
                } catch(Exception e) {
                    Debug.Log(e.Message);
                }
                break;
            }
 
            var valueRect = position;
            // valueRect.x = position.width / 2f + 15f;
            valueRect.x = keyRect.xMax + 15f;
            valueRect.width = position.xMax - valueRect.x - kButtonWidth;
            EditorGUI.BeginChangeCheck();
            if (typeof(TV) == typeof(DInt)) extra = DIntDrawer.GetPropertyHeight(value as DInt);
            else if (typeof(TV) == typeof(DLong)) extra = DLongDrawer.GetPropertyHeight(value as DLong);
            else if (typeof(TV) == typeof(DFloat)) extra = DFloatDrawer.GetPropertyHeight(value as DFloat);
            else extra = 17f;
            
            value = DoField(valueRect, typeof(TV), value, property);
            if (EditorGUI.EndChangeCheck()) {
                _Dictionary[key] = value;
                break;
            }
 
            var removeRect = valueRect;
            removeRect.x = valueRect.xMax + 2;
            removeRect.width = kButtonWidth;
            if (GUI.Button(removeRect, new GUIContent("x", "Remove item"), EditorStyles.miniButtonRight)) {
                RemoveItem(key);
                break;
            }
        }
    }
 
    private void RemoveItem(TK key) => _Dictionary.Remove(key);

    private void CheckInitialize(SerializedProperty property, GUIContent label) {
        if (_Dictionary == null) {
            var target = property.serializedObject.targetObject;
            _Dictionary = fieldInfo.GetValue(target) as SerializableDictionary<TK, TV>;
            if (_Dictionary == null) {
                _Dictionary = new SerializableDictionary<TK, TV>();
                fieldInfo.SetValue(target, _Dictionary);
            }
 
            _Foldout = EditorPrefs.GetBool(label.text);
        }
    }

    private static readonly Dictionary<Type, Func<Rect, object, object>> _Fields = new Dictionary<Type,Func<Rect,object,object>> {
        { typeof(int), (rect, value) => EditorGUI.IntField(rect, (int)value) },
        { typeof(float), (rect, value) => EditorGUI.FloatField(rect, (float)value) },
        { typeof(string), (rect, value) => EditorGUI.TextField(rect, (string)value) },
        { typeof(bool), (rect, value) => EditorGUI.Toggle(rect, (bool)value) },
        { typeof(Vector2), (rect, value) => EditorGUI.Vector2Field(rect, GUIContent.none, (Vector2)value) },
        { typeof(Vector3), (rect, value) => EditorGUI.Vector3Field(rect, GUIContent.none, (Vector3)value) },
        { typeof(Bounds), (rect, value) => EditorGUI.BoundsField(rect, (Bounds)value) },
        { typeof(Rect), (rect, value) => EditorGUI.RectField(rect, (Rect)value) }
    };

    private static T DoField<T>(Rect rect, Type type, T value, SerializedProperty serializedProperty = null) {
        Func<Rect, object, object> field;
        if (_Fields.TryGetValue(type, out field)) return (T)field(rect, value);

        if (type.IsEnum) return (T)(object)EditorGUI.EnumPopup(rect, (Enum)(object)value);

        if (typeof(UnityObject).IsAssignableFrom(type)) return (T)(object)EditorGUI.ObjectField(rect, (UnityObject)(object)value, type, true);

        if (type == typeof(DInt)) return (T)(object)DIntDrawer.OnGUI(rect, (DInt)(object)value, GUIContent.none, serializedProperty.serializedObject);
        
        if (type == typeof(DLong)) return (T)(object)DLongDrawer.OnGUI(rect, (DLong)(object)value, GUIContent.none, serializedProperty.serializedObject);
        
        if (type == typeof(DFloat)) return (T)(object)DFloatDrawer.OnGUI(rect, (DFloat)(object)value, GUIContent.none, serializedProperty.serializedObject);
        
        Debug.Log("Type is not supported: " + type);
        return value;
    }

    private void ClearDictionary() => _Dictionary.Clear();

    private void AddNewItem() {
        TK key;
        if (typeof(TK) == typeof(string)) key = (TK)(object)"";
        else key = default(TK);

        var value = default(TV);
        try {
            _Dictionary.Add(key, value);
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }
}

[CustomPropertyDrawer(typeof(StringDIntDictionary))]
public class StringDIntDrawer : DictionaryDrawer<string, DInt> { }

[CustomPropertyDrawer(typeof(StringDLongDictionary))]
public class StringDLongDrawer : DictionaryDrawer<string, DLong> { }

[CustomPropertyDrawer(typeof(StringDFloatDictionary))]
public class StringDFloatDrawer : DictionaryDrawer<string, DFloat> { }