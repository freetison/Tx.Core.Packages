﻿using System;
using System.Reflection;

namespace Tx.Core.Extensions.Assembly;

public static class ApplicationInfo
{
    public static Version Version => System.Reflection.Assembly.GetCallingAssembly().GetName().Version;

    public static string Title
    {
        get
        {
            object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title.Length > 0) return titleAttribute.Title;
            }
            return System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
    }

    public static string ProductName
    {
        get
        {
            object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    public static string Description
    {
        get
        {
            object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    public static string CopyrightHolder
    {
        get
        {
            object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    public static string CompanyName
    {
        get
        {
            object[] attributes = System.Reflection.Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }

}