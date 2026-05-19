using Godot;

namespace ZeusInspector.Editor;

public static class EditorUtils
{
    public static T Get<[MustBeVariant] T>(this GodotObject obj, string propertyName)
    {
        return obj.Get(propertyName).As<T>();
    }
}