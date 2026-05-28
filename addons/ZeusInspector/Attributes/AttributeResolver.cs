using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace ZeusInspector.Attributes;

public static class AttributeResolver
{

    public static Type ResolveActualType(GodotObject target)
    {
        var type = target.GetType();

        var script = target.GetScript().As<Script>();
        if (script != null && !string.IsNullOrEmpty(script.ResourcePath))
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(script.ResourcePath);
            var resolved = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == className);
            if (resolved != null)
                return resolved;
        }

        return type;
    }

    public static List<InspectorAttribute> GetAttributes(GodotObject target, string propertyName)
    {
        var result = new List<InspectorAttribute>();
        var type = ResolveActualType(target);

        var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            result.AddRange(field.GetCustomAttributes<InspectorAttribute>());
            return result;
        }

        var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null)
        {
            result.AddRange(prop.GetCustomAttributes<InspectorAttribute>());
            return result;
        } 

        return result;
    }
}
