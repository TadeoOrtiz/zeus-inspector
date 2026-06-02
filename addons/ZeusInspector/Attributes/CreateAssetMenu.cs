using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CreateAssetMenu<T>(string path) : Attribute where T : Resource
{
    public string Path { get; } = path;
}