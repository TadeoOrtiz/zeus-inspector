using Godot;
using ZeusInspector.Attributes;

[GlobalClass]
[CreateAssetMenu<TestStruct>("Test/TestStruct")]
public partial class TestStruct : Resource
{
    [Export]
    public int test;
}