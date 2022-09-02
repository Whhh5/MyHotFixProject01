using System;

public static class LinkEditor
{
    public static Type GetScriteType(string name)
    {
        Type type = Type.GetType(name);
        return type;
    }
}
