using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ServiceScript
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EditID : Attribute
    {
        public string ID { get; }

        public EditID(string str) => ID = str;
        
        private static BindingFlags Flag => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [CanBeNull] public static void SetValue(object target, object value, string id, Data data)
        {
            if (data == Data.Field)
            {
                var f = GetField(target, id);
                if (f != null) f.SetValue(target, value);
            }
            else
            {
                var p = GetProperty(target, id);
                if (p != null) p.SetValue(target, value);
            }
        }
        
        [CanBeNull] public static object GetValue(object target, string id, Data data)
        {
            if (data == Data.Field)
            {
                var f = GetField(target, id);
                if (f != null) return f.GetValue(target);
                else return null;
            }
            else
            {
                var p = GetProperty(target, id);
                if (p != null) return p.GetValue(target);
                else return null;
            }
        }

        [CanBeNull] public static FieldInfo GetField(object target, string id)
        {
            return target.GetType().GetFields(Flag).FirstOrDefault(x =>
            {
                var atr = x.GetCustomAttribute<EditID>();
                if (atr != null)
                    if (atr.ID == id)
                        return true;
                return false;
            });
        }
        
        [CanBeNull] public static PropertyInfo GetProperty(object target, string id)
        {
            return target.GetType().GetProperties(Flag).FirstOrDefault(x =>
            {
                var atr = x.GetCustomAttribute<EditID>();
                if (atr != null)
                    if (atr.ID == id)
                        return true;
                return false;
            });
        }
        
        public enum Data
        {
            Field, Property
        }
    }
}
