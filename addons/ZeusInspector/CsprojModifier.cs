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

    /// <summary>
    /// Añade de forma segura la etiqueta <Import Project="..." /> si no existe.
    /// </summary>
    public static void AddImport()
    {
        try
        {
            string csprojPath = GetCsprojPath();
            XDocument doc = XDocument.Load(csprojPath);
            XElement root = doc.Root;

            if (root == null) return;

            // Buscamos si ya existe un Import con esa misma ruta para no duplicarlo
            bool alreadyExists = root.Elements()
                .Any(e => e.Name.LocalName == "Import" &&
                          e.Attribute("Project")?.Value == PropsRelativePath);

            if (alreadyExists)
            {
                return;
            }

            // Creamos el nuevo elemento <Import Project="la_ruta" />
            // Usamos root.Name.Namespace para mantener el mismo namespace XML del documento
            XElement importElement = new XElement(root.Name.Namespace + "Import",
                new XAttribute("Project", PropsRelativePath)
            );

            // Lo añadimos como último elemento del proyecto
            root.Add(importElement);
            doc.Save(csprojPath);

            GD.Print("[ZeusInspector] .props added to .csproj.");
        }
        catch (Exception e)
        {
            GD.PrintErr($"[ZeusInspector] Error to Import .props to .csproj: {e.Message}");
        }
    }

    /// <summary>
    /// Quita la etiqueta <Import Project="..." /> si existe en el archivo.
    /// </summary>
    public static void RemoveImport()
    {
        try
        {
            string csprojPath = GetCsprojPath();
            XDocument doc = XDocument.Load(csprojPath);
            XElement root = doc.Root;

            if (root == null) return;

            // Buscamos todos los elementos <Import> que apunten a nuestro archivo .props
            var importsToRemove = root.Elements()
                .Where(e => e.Name.LocalName == "Import" &&
                            e.Attribute("Project")?.Value == PropsRelativePath)
                .ToList();

            if (importsToRemove.Count == 0)
            {
                GD.Print("[ZeusInspector] .props not found to remove.");
                return;
            }

            // Los removemos del árbol XML
            foreach (var import in importsToRemove)
            {
                import.Remove();
            }

            doc.Save(csprojPath);
            GD.Print("[ZeusInspector] .props removed from .csproj.");
        }
        catch (Exception e)
        {
            GD.PrintErr($"[MiAddon] Error al intentar remover el Import del .csproj: {e.Message}");
        }
    }

    /// <summary>
    /// Utilidad para encontrar automáticamente el archivo .csproj en la raíz de Godot.
    /// </summary>
    private static string GetCsprojPath()
    {
        // ProjectSettings.GlobalizePath("res://") nos da la ruta real en el disco (C:/Proyectos/MiJuego/)
        string globalResPath = ProjectSettings.GlobalizePath("res://");

        // Buscamos cualquier archivo que termine en .csproj en la raíz
        string[] csprojFiles = Directory.GetFiles(globalResPath, "*.csproj");

        if (csprojFiles.Length == 0)
        {
            throw new FileNotFoundException("No se encontró ningún archivo .csproj en la raíz del proyecto.");
        }

        return csprojFiles[0]; // Retorna el primero que encuentre
    }
}
