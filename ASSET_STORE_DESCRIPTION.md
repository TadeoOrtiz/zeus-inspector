# Zeus Inspector

## Brief Description

Extend Godot's Inspector with C# attributes to group, organize, and customize exported properties without writing editor scripts.

---

## Full Description

**Zeus Inspector** is a Godot 4 plugin that enhances the default inspector through custom C# attributes. Forget writing `EditorProperty` or `EditorInspectorPlugin` for every property — just decorate your `[Export]` fields with attributes and the plugin handles the rest.

### Features

- **Horizontal & Vertical Groups** — Group related properties into rows or columns with nesting support via `/`.
- **Hide Labels** — `[HideLabel]` removes the property label for more compact layouts.
- **Label Ratio** — `[LabelRatio]` adjusts the split ratio between the property name and its value editor.
- **Preview Field** — `[PreviewField]` shows a resource preview directly in the inspector.
- **Custom Editor** — `[CustomEditor<T>]` injects any custom `EditorProperty` into a specific property.
- **Custom Docks** — Create entire dock panels that auto-appear when selecting a specific node or resource type, using `[CustomDock]` and extending `CustomDock`.
- **Zero configuration** — Just enable the plugin and attributes work instantly.

### Usage Example

```csharp
public partial class Item : Resource
{
    [Export] public string Name;

    [HorizontalGroup("Stats", "Stats")]
    [Export] public int Damage;

    [HorizontalGroup("Stats")]
    [Export] public int Durability;

    [Export] public int Cost;
}
```

This places `Damage` and `Durability` side by side under a "Stats" header, while `Name` and `Cost` display normally.

### Custom Docks

```csharp
[CustomDock(typeof(Item))]
public class ItemCustomDock : CustomDock
{
    public override Control CreateInspectorGUI()
    {
        var label = new Label { Text = (string)Target.Get("name") };
        return label;
    }
}
```

### Requirements

- Godot 4.7+
- C# (.NET 8.0+)
- Mono or .NET-enabled editor

### License

MIT
