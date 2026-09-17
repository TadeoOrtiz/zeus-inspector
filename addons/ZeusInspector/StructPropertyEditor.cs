#if TOOLS 
using Godot;
using Godot.Collections;

namespace ZeusInspector;


[Tool]
public partial class StructEditorProperty : EditorProperty
{

  private VBoxContainer propertyContainer;

  public StructEditorProperty()
  {
    // var godotObj = GetEditedObject();
    // var prop = godotObj.Get();
    UseFolding = true;
    Keying = true;


    propertyContainer = new();
    AddChild(propertyContainer);

  }

  public override void _UpdateProperty()
  {

    var structProp = GetEditedObject().Get(GetEditedProperty()).As<Dictionary<string, Variant>>();
    foreach (var (propName, propValue) in structProp)
    {
      if (propName.StartsWith('_')) continue;

      var editor = EditorInspector.InstantiatePropertyEditor(
          GetEditedObject(),
          propValue.VariantType,
          $"{GetEditedProperty()}:{propName}",
          PropertyHint.None,
          "",
          (uint)PropertyUsageFlags.None
      );
      // GD.Print(GetEditedObject().GetIndexed($"{GetEditedProperty()}:{propName}"));
      //propertyContainer.AddChild(new Label() { Text = $"{propName} - {propValue}" });
      editor.SetObjectAndProperty(GetEditedObject(), $"{GetEditedProperty()}:{propName}");
      editor.Label = propName;
      //editor.Name = propName;
      editor.UpdateProperty();
      propertyContainer.AddChild(editor);
      // editor.Keying = true;
    }
  }

}
#endif