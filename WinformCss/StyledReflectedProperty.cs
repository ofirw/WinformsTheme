using System;
using System.Drawing;
using System.Reflection;

namespace WinformsTheme
{

    public class StyledReflectedProperty
    {
        private readonly object instance;
        private readonly PropertyInfo propertyInfo;

        public static StyledReflectedProperty Create(object control, string key)
        {
            var props = key.Split('.');
            PropertyInfo pi = null;
            object currentInstance = control;
            object previousInstance = control;
            var currentTarget = currentInstance.GetType();
            foreach (var p in props)
            {
                previousInstance = currentInstance;
                pi = currentTarget.GetProperty(p, BindingFlags.Instance | BindingFlags.Public);
                if (pi == null) break;
                currentInstance = pi.GetValue(previousInstance, null);
                currentTarget = pi.PropertyType;
            }
            currentInstance = previousInstance;
            if (pi == null) return null;

            return new StyledReflectedProperty(currentInstance, pi);
        }

        private StyledReflectedProperty(object currentInstance, PropertyInfo propertyInfo)
        {
            this.instance = currentInstance;
            this.propertyInfo = propertyInfo;
        }

        public void SetValue(string args) => SetValue(args.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        public void SetValue(params string[] args)
        {
            Type propertyType = this.propertyInfo.PropertyType;
            if (propertyType == typeof(Color))
            {
                SetColor(args[0]);
            }
            else if (propertyType.IsPrimitive)
            {
                var primitive = Convert.ChangeType(args[0], propertyType);
                this.SetValueCore(primitive);
            }
            else if (propertyType.IsEnum)
            {
                var enumValue = Enum.Parse(propertyType, args[0], true);
                this.SetValueCore(enumValue);
            }
            else
            {
                SetViaCtor(args);
            }
        }

        private void SetViaCtor(string[] args)
        {
            foreach (ConstructorInfo ci in this.propertyInfo.PropertyType.GetConstructors())
            {
                ParameterInfo[] ctorArgs = ci.GetParameters();
                if (ctorArgs.Length != args.Length)
                    continue;

                object[] values = new object[args.Length];
                bool good = true;
                for (int i = 0; i < ctorArgs.Length; i++)
                {
                    try
                    {
                        Type argType = ctorArgs[i].ParameterType;
                        if (argType.IsEnum)
                        {
                            values[i] = Enum.Parse(argType, args[i], true);
                        }
                        else
                        {
                            values[i] = Convert.ChangeType(args[i], ctorArgs[i].ParameterType);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is ArgumentException || ex is ArgumentNullException || ex is InvalidCastException)
                        {
                            good = false;
                            break;
                        }
                        throw;
                    }
                }

                if (good)
                {
                    object value = ci.Invoke(values);
                    this.SetValueCore(value);
                    break;
                }
            }
        }

        private void SetColor(string color)
        {
            object value;
            if (color[0] == '#')
            {
                value = ColorTranslator.FromHtml(color);
            }
            else
            {
                value = Color.FromKnownColor(
                    (KnownColor)Enum.Parse(typeof(KnownColor), color, true));
            }

            this.SetValueCore(value);
        }

        private void SetValueCore(object value)
        {
            this.propertyInfo.SetValue(this.instance, value, null);
        }
    }
}