﻿using System;

namespace Xciles.PclValueInjecter.Extensions
{
    public static class StringExtensions
    {
        public static string RemovePrefix(this string o, string prefix)
        {
            return o.RemovePrefix(prefix, StringComparison.Ordinal);
        }

        public static string RemovePrefix(this string o, string prefix, StringComparison comparison)
        {
            if (prefix == null) return o;
            return !o.StartsWith(prefix, comparison) ? o : o.Remove(0, prefix.Length);
        }

        public static string RemoveSuffix(this string o, string suffix)
        {
            if(suffix == null) return o;
            return !o.EndsWith(suffix) ? o : o.Remove(o.Length - suffix.Length, suffix.Length);
        }
    }
}