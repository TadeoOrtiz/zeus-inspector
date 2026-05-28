using System;
using Godot;

namespace ZeusInspector.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class VerticalGroupAttribute(string groupName, bool showTitle = false) : GroupAttribute(groupName, GroupOrientation.Vertical, showTitle) { }
