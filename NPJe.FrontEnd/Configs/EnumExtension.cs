using NPJe.FrontEnd.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace NPJe.FrontEnd.Configs
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return null; // could also return string.Empty
        }


        public static List<GenericInfoComboDto> GetList<T>() where T : IConvertible
        {
            var especialidadeList = new List<GenericInfoComboDto>();

            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                especialidadeList.Add(new GenericInfoComboDto(Convert.ToInt32(enumValue), enumValue.GetDescription()));
            }
            return especialidadeList;
        }
    }
}