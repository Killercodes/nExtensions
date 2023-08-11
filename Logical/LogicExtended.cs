public static class LogicExt
{
    public static void Then(this bool condition, Action conditionalAction)
    {
        if(condition)
            conditionalAction.Invoke();
    }

    public static void Then<T>(this bool condition,T t, Action<T> conditionalAction)
    {
        if (condition)
            conditionalAction.Invoke(t);
    }

    public static T Then<T>(this bool condition, T t, Func<T,T> conditionalAction)
    {
        if (condition)
            return conditionalAction.Invoke(t);

        return default(T);
    }

}
