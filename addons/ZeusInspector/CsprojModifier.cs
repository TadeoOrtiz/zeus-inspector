using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Godot;

namespace ZeusInspector;

public static class CsprojModifier
{
  // La ruta relativa al archivo .props desde la raíz del proyecto
  private const string PropsRelativePath = @"addons\ZeusInspector\ZeusInspector.props";

  public static void EnableGodotGenerators()
  {
    try
    {
      string csprojPath = GetCsprojPath();
      XDocument doc = XDocument.Load(csprojPath);
      XElement root = doc.Root;

      if (root == null) return;

      var tagElements = root.Descendants()
          .Where(e => e.Name.LocalName == "DisableImplicitGodotGeneratorReferences")
          .ToList();

      if (tagElements.Count == 0)
      {
        GD.Print("[ZeusInspector] DisableImplicitGodotGeneratorReferences no se encontró para remover.");
        return;
      }

      foreach (var tag in tagElements)
      {
        XElement parent = tag.Parent;
        tag.Remove();

        // Si el PropertyGroup queda vacío, lo removemos también
        if (parent != null && parent.Name.LocalName == "PropertyGroup" && !parent.HasElements)
        {
          parent.Remove();
        }
      }

      doc.Save(csprojPath);
      GD.Print("[ZeusInspector] DisableImplicitGodotGeneratorReferences removido del .csproj.");
    }
    catch (Exception e)
    {
      GD.PrintErr($"[ZeusInspector] Error al remover DisableImplicitGodotGeneratorReferences: {e.Message}");
    }
  }

  public static void DisableGodotGenetarors()
  {
    try
    {
      string csprojPath = GetCsprojPath();
      XDocument doc = XDocument.Load(csprojPath);
      XElement root = doc.Root;

      if (root == null) return;

      XNamespace ns = root.Name.Namespace;

      // Buscar la etiqueta existente
      XElement tagElement = root.Descendants()
          .FirstOrDefault(e => e.Name.LocalName == "DisableImplicitGodotGeneratorReferences");

      if (tagElement != null)
      {
        if (tagElement.Value == "true")
        {
          GD.Print("[ZeusInspector] DisableImplicitGodotGeneratorReferences ya está activado.");
          return;
        }
        tagElement.Value = "true";
      }
      else
      {
        // Si no existe la etiqueta, buscamos el primer PropertyGroup o creamos uno nuevo
        XElement propertyGroup = root.Elements().FirstOrDefault(e => e.Name.LocalName == "PropertyGroup");

        if (propertyGroup == null)
        {
          propertyGroup = new XElement(ns + "PropertyGroup");
          root.AddFirst(propertyGroup);
        }

        propertyGroup.Add(new XElement(ns + "DisableImplicitGodotGeneratorReferences", "true"));
      }

      doc.Save(csprojPath);
      GD.Print("[ZeusInspector] DisableImplicitGodotGeneratorReferences establecido a true.");
    }
    catch (Exception e)
    {
      GD.PrintErr($"[ZeusInspector] Error al activar DisableImplicitGodotGeneratorReferences: {e.Message}");
    }
  }


  public static void AddImportAndItemGroup()
  {
    try
    {
      string csprojPath = GetCsprojPath();
      XDocument doc = XDocument.Load(csprojPath);
      XElement root = doc.Root;

      if (root == null) return;

      bool alreadyExists = root.Elements()
          .Any(e => e.Name.LocalName == "Import" &&
                    e.Attribute("Project")?.Value == PropsRelativePath);

      if (alreadyExists)
      {
        return;
      }

      XElement importElement = new XElement(root.Name.Namespace + "Import",
          new XAttribute("Project", PropsRelativePath)
      );

      XNamespace ns = root.Name.Namespace;

      XElement itemGroup = new XElement(ns + "ItemGroup",
          new XElement(ns + "Compile",
              new XAttribute("Remove", "ZeusInspector.Generators/**/*.cs")
          ),
          new XElement(ns + "Compile",
              new XAttribute("Remove", ".godot/mono/temp/obj/Debug/**/*.cs")
          )
      );

      // Lo añadimos como último elemento del proyecto
      root.Add(importElement);
      root.Add(itemGroup);
      doc.Save(csprojPath);

      GD.Print("[ZeusInspector] .props added to .csproj.");
    }
    catch (Exception e)
    {
      GD.PrintErr($"[ZeusInspector] Error to Import .props to .csproj: {e.Message}");
    }
  }

  public static void RemoveImportAndItemGroup()
  {
    try
    {
      string csprojPath = GetCsprojPath();
      XDocument doc = XDocument.Load(csprojPath);
      XElement root = doc.Root;

      if (root == null) return;

      bool isModified = false;

      var importsToRemove = root.Elements()
          .Where(e => e.Name.LocalName == "Import" &&
                      e.Attribute("Project")?.Value == PropsRelativePath)
          .ToList();

      foreach (var import in importsToRemove)
      {
        import.Remove();
        isModified = true;
      }

      var compilesToRemove = root.Descendants()
          .Where(e => e.Name.LocalName == "Compile" &&
                     (e.Attribute("Remove")?.Value == "ZeusInspector.Generators/**/*.cs" ||
                      e.Attribute("Remove")?.Value == ".godot/mono/temp/obj/Debug/**/*.cs"))
          .ToList();

      foreach (var compile in compilesToRemove)
      {
        XElement parent = compile.Parent;
        compile.Remove();
        isModified = true;

        if (parent != null && parent.Name.LocalName == "ItemGroup" && !parent.HasElements)
        {
          parent.Remove();
        }
      }

      if (isModified)
      {
        doc.Save(csprojPath);
        GD.Print("[ZeusInspector] Import e ItemGroup removidos exitosamente del .csproj.");
      }
      else
      {
        GD.Print("[ZeusInspector] No se encontraron elementos para remover en el .csproj.");
      }
    }
    catch (Exception e)
    {
      GD.PrintErr($"[ZeusInspector] Error al intentar remover la configuración del .csproj: {e.Message}");
    }
  }

  private static string GetCsprojPath()
  {
    string globalResPath = ProjectSettings.GlobalizePath("res://");

    string[] csprojFiles = Directory.GetFiles(globalResPath, "*.csproj");

    if (csprojFiles.Length == 0)
    {
      throw new FileNotFoundException("No se encontró ningún archivo .csproj en la raíz del proyecto.");
    }

    return csprojFiles[0]; // Retorna el primero que encuentre
  }
}
