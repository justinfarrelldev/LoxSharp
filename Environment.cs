using System.Collections.Generic;

public class Environment
{
    private Dictionary<string, object> values = new Dictionary<string, object>();
    public void define(string name, object value)
    {
        values.Add(name, value);
    }

    public object get(LoxSharp.Token name)
    {
        if (values.ContainsKey(name.lexeme))
        {
            return values[name.lexeme];
        }

        throw new Errors.RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
    }

    public void assign(LoxSharp.Token name, object value)
    {
        if (values.ContainsKey(name.lexeme))
        {
            values[name.lexeme] = value;
            return;
        }

        throw new Errors.RuntimeError(name, $"Undefined variable '{name.lexeme}.'");
    }
}