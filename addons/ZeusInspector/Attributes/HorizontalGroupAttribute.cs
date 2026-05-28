using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class HorizontalGroupAttribute(string groupName, string Title = "") : GroupAttribute(groupName, GroupOrientation.Horizontal, Title) { }
