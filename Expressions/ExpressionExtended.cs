using System;

namespace nExtensions
{

public class VarInfo
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public Type VType { get; private set; }
        public VarInfo(string name,object value)
        {
            Name = name;
            Value = value;
            VType = value.GetType();
        }
    }
    public static class GetInfo
    {
        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static VarInfo GetParameterInfo<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            var _value = memberExpression.Compile()();
            var _name = expressionBody.Member.Name;

            return new VarInfo(name: _name, value: _value);
        }


        public static string CheckVariable<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            var _value = memberExpression.Compile();
            var _name = expressionBody.Member.Name;

            if (_value() == null)
            {
                return string.Format("CheckVariable:NULL, {0} is NULL", _name);
            }
            else if (_value().GetType() == typeof(string) && _value().ToString().Length == 0)
            {
                return string.Format("CheckVariable:EMPTY, {0} is EMPTY", _name); 
            }
            else
            {
                return string.Format("CheckVariable:OK, {0} = {1}", _name, _value());
            }
        }




}
