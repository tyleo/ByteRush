namespace ByteRush.Graph
{
    public delegate void CodeGenerator(
        in Node node,
        in NodeDef nodeDef,
        in CodeGen.State state,
        in State graphState
    );
}
