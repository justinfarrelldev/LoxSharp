using System.Collections.Generic;

public class Environment
{
    private Environment enclosing;
    public Environment()
    {
        enclosing = null;
    }

    public Environment(Environment enclosing)
    {
        this.enclosing = enclosing;
    }
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

        if (enclosing != null) return enclosing.get(name);

        throw new Errors.RuntimeError(name, $"Undefined variable '{name.lexeme}'.");
    }

    public void assign(LoxSharp.Token name, object value)
    {
        if (values.ContainsKey(name.lexeme))
        {
            values[name.lexeme] = value;
            return;
        }

        if (enclosing != null)
        {
            enclosing.assign(name, value);
            return;
        }

        throw new Errors.RuntimeError(name, $"Undefined variable '{name.lexeme}.'");
    }
}