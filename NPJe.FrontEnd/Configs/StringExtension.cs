﻿using System.Globalization;
using System.Text;

namespace NPJe.FrontEnd.Configs
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string e) { 
            return e == null || (e != null && e.Trim().Length == 0);
        }

        public static string RemoveAccents(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }


    }
}