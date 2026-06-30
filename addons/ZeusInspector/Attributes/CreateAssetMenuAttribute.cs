using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CreateAssetMenuAttribute(string menuName = "") : Attribute
{
    public string MenuName { get; } = menuName;
}