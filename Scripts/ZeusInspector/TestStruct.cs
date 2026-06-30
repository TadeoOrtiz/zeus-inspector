using Godot;
using ZeusInspector.Attributes;

[GlobalClass]
[Icon("res://Scripts/node/sword.svg")]
[CreateAssetMenu("Data")]
public partial class TestStruct : Resource
{
    /// <summary>
    /// This is a test variable to demonstrate the custom inspector functionality.
    /// </summary>
    [Export]
    public int test;

    /// <summary>  
    /// Descripción breve de la clase o método.  
    /// </summary>  
    /// <param name="parametro">Descripción del parámetro.</param>  
    /// <returns>Descripción del valor de retorno.</returns>  
    public void MiMetodo(string parametro)
    {
        // Implementación
    }
}