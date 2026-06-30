using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using ZeusInspector.Attributes;

namespace ZeusInspector;

public partial class AssetContextMenu : EditorContextMenuPlugin
{
    private readonly List<Callable> _menuCallables = new();
    private string targetFolder;

    private class MenuNode
    {
        public string Segment { get; set; }
        public List<Type> Types { get; } = new();
        public List<MenuNode> Children { get; } = new();
    }

    private static Texture2D GetTypeIcon(Theme theme, Type type)
    {
        Texture2D icon = null;

        var iconAttr = type.GetCustomAttribute<IconAttribute>();
        if (iconAttr != null)
            icon = ResourceLoader.Load<Texture2D>(iconAttr.Path);

        if (icon == null)
        {
            var current = type;
            while (current != null)
            {
                if (theme.HasIcon(current.Name, "EditorIcons"))
                {
                    icon = theme.GetIcon(current.Name, "EditorIcons");
                    break;
                }
                current = current.BaseType;
            }
        }

        icon ??= theme.GetIcon("Object", "EditorIcons");

        if (icon.GetWidth() > 20 || icon.GetHeight() > 20)
        {
            var image = icon.GetImage();
            if (image != null)
            {
                var scale = Math.Min(20f / icon.GetWidth(), 20f / icon.GetHeight());
                var w = Math.Max(1, (int)(icon.GetWidth() * scale));
                var h = Math.Max(1, (int)(icon.GetHeight() * scale));
                image.Resize(w, h, Image.Interpolation.Bilinear);
                icon = ImageTexture.CreateFromImage(image);
            }
        }

        return icon;
    }

    public void ClearMenu()
    {
        _menuCallables.Clear();
    }

    public override void _PopupMenu(string[] paths)
    {
        if (paths.Length == 0)
        {
            targetFolder = EditorInterface.Singleton.GetCurrentDirectory();
        }
        else
        {
            targetFolder = paths[0];
        }
        Assembly assembly = Assembly.GetExecutingAssembly();

        var results = assembly.GetTypes()
            .Where(t => t.IsClass && t.IsDefined(typeof(CreateAssetMenuAttribute), false))
            .Select(t => new
            {
                type = t,
                attr = (CreateAssetMenuAttribute)Attribute.GetCustomAttribute(t, typeof(CreateAssetMenuAttribute))
            })
            .ToList();

        var root = new MenuNode();
        var theme = EditorInterface.Singleton.GetEditorTheme();
        var folderIcon = theme.GetIcon("Folder", "EditorIcons");

        foreach (var item in results)
        {
            var segments = string.IsNullOrEmpty(item.attr.MenuName)
                ? []
                : item.attr.MenuName.Split('/', StringSplitOptions.RemoveEmptyEntries);

            var current = root;

            if (!item.type.IsSubclassOf(typeof(Resource))) continue;

            foreach (var segment in segments)
            {
                var child = current.Children.FirstOrDefault(c => c.Segment == segment);
                if (child == null)
                {
                    child = new MenuNode { Segment = segment };
                    current.Children.Add(child);
                }
                current = child;
            }
            current.Types.Add(item.type);
        }

        _menuCallables.Clear();
        int nextId = 0;

        foreach (var type in root.Types)
        {
            var callable = Callable.From<string[]>((_p) => CreateAsset(type));
            _menuCallables.Add(callable);
            AddContextMenuItem(type.Name, callable, GetTypeIcon(theme, type));
        }

        foreach (var child in root.Children)
        {
            var popup = new PopupMenu();
            popup.AddThemeConstantOverride("icon_max_width", 20);
            var typeById = new Dictionary<int, Type>();
            BuildMenu(child, popup, typeById, ref nextId, theme, folderIcon);
            popup.IdPressed += (pressedId) =>
            {
                if (typeById.TryGetValue((int)pressedId, out var type))
                {
                    CreateAsset(type);
                }
            };

            AddContextSubmenuItem(child.Segment, popup, folderIcon);
        }
    }

    private void BuildMenu(MenuNode node, PopupMenu menu, Dictionary<int, Type> typeById, ref int nextId, Theme theme, Texture2D folderIcon)
    {
        foreach (var child in node.Children)
        {
            var childPopup = new PopupMenu();
            childPopup.AddThemeConstantOverride("icon_max_width", 20);
            var childTypeById = new Dictionary<int, Type>();
            BuildMenu(child, childPopup, childTypeById, ref nextId, theme, folderIcon);
            childPopup.IdPressed += (pressedId) =>
            {
                if (childTypeById.TryGetValue((int)pressedId, out var type))
                {
                    CreateAsset(type);
                }
            };

            menu.AddSubmenuNodeItem(child.Segment, childPopup);
            menu.SetItemIcon(menu.ItemCount - 1, folderIcon);
        }

        foreach (var type in node.Types)
        {
            var id = nextId++;
            menu.AddItem(type.Name, id);
            menu.SetItemIcon(menu.ItemCount - 1, GetTypeIcon(theme, type));
            typeById[id] = type;
        }
    }


    private void CreateAsset(Type type)
    {
        var targetDir = targetFolder;
        if (DirAccess.Open(targetDir) == null)
            targetDir = targetDir.GetBaseDir();
        if (!targetDir.EndsWith("/"))
            targetDir += "/";

        var basePath = targetDir + type.Name;
        var filePath = basePath + ".tres";
        var counter = 0;
        while (FileAccess.FileExists(filePath))
        {
            counter++;
            filePath = $"{basePath}_{counter}.tres";
        }

        if (Activator.CreateInstance(type) is not Resource resource)
        {
            GD.Print($"Cannot create resource of type {type.Name}");
            return;
        }

        var error = ResourceSaver.Save(resource, filePath);
        if (error == Error.Ok)
            EditorInterface.Singleton.SelectFile(filePath);
        else
            GD.Print($"Error saving {filePath}: {error}");
    }
}