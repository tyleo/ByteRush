namespace ByteRush.Graph
{
    public delegate void CodeGenerator(
        in Node node,
        in FunctionDef function,
        in CodeGen.CodeGenState state,
        in State graphState
    );
}
