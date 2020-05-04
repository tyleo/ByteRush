namespace ByteRush.Graph
{
    public delegate void CodeGenerator(
        in Node node,
        in NodeDef nodeDef,
        in CodeGen.CodeOnlyState state,
        in State graphState
    );
}
