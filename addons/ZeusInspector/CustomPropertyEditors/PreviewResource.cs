using Godot;
using ZeusInspector.Attributes;

public partial class PreviewResource : EditorProperty
{
    private EditorResourcePicker editorPicker;

    public PreviewResource() { }

    public PreviewResource(string baseType, int size)
    {
        editorPicker = new EditorResourcePicker
        {
            BaseType = baseType,
            CustomMinimumSize = new(0, size),
            //CustomMaximumSize = new(-1, size),
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
            SizeFlagsVertical = SizeFlags.ExpandFill
        };


        editorPicker.ResourceChanged += OnResourceChanged;
        AddChild(editorPicker);
        AddFocusable(editorPicker);
    }

    private void OnResourceChanged(Resource resource)
    {
        EmitChanged(GetEditedProperty(), resource);
    }

    public override void _UpdateProperty()
    {
        editorPicker.EditedResource = (Resource)(GodotObject)GetEditedObject().Get(GetEditedProperty());
    }


}