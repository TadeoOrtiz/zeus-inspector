using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class LabelRatioAttribute(float ratio) : InspectorAttribute
{
    public override void ParseEditor(EditorProperty editor)
    {
        editor.NameSplitRatio = ratio;
    }

}


