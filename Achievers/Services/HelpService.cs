using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Achievers.Services
{
    public static class HelpService
    {
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static DependencyObject FindAncestorByName(DependencyObject el, string name)
        {
            var parent = VisualTreeHelper.GetParent(el);
            if (parent != null)
            {
                if (parent.GetValue(FrameworkElement.NameProperty).ToString() == name)
                {
                    return parent;
                }
                else
                {
                    return FindAncestorByName(parent, name);
                }
            }
            return null;
        }

        public static T FindAncestorByType<T>(DependencyObject el)
        {
            object parent = VisualTreeHelper.GetParent(el);
            if (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                else
                {
                    return FindAncestorByType<T>((DependencyObject)parent);
                }
            }
            return default(T);
        }

        public static DependencyObject FindChildByName(DependencyObject el, string name)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(el);
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(el, i);
                    if (child.GetValue(FrameworkElement.NameProperty).ToString() == name)
                    {
                        return child;
                    }
                }
                for (int i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(el, i);
                    var result = FindChildByName(child, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public static T FindChildByType<T>(DependencyObject el)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(el);
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    object child = VisualTreeHelper.GetChild(el, i);
                    if (child is T)
                    {
                        return (T)child;
                    }
                }
                for (int i = 0; i < childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(el, i);
                    var result = FindChildByType<T>(child);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return default(T);
        }

        public static string RemoveSpecialCharacter(string text)
        {
            return Regex.Replace(text, "’", "");
        }

        public static string ClearLineBreak(string text)
        {
            return Regex.Replace(text, "\\n|\n", "");
        }

        public static List<T> ShuffleList<T>(List<T> arr)
        {
            T[] list = new T[arr.Count];
            arr.CopyTo(list);
            var r = new Random();
            for (int i = 0, len = arr.Count; i < len; i++)
            {
                int newIndex = r.Next(0, len - 1);
                var temp = list[i];
                list[i] = list[newIndex];
                list[newIndex] = temp;
            }
            return list.ToList();
        }
    }
}