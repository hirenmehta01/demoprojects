using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public static class Extention
{
    public static string ToString<T>(this IList<T> list, string include = "", string exclude = "")
    {
        //Variables for build string
        string propStr = string.Empty;
        StringBuilder sb = new StringBuilder();

        //Get property collection and set selected property list
        PropertyInfo[] props = typeof(T).GetProperties();
        List<PropertyInfo> propList = GetSelectedProperties(props, include, exclude);

        //Add list name and total count
        string typeName = GetSimpleTypeName(list);
        sb.AppendLine(string.Format("{0} List - Total Count: {1}", typeName, list.Count.ToString()));

        //Iterate through data list collection
        foreach (var item in list)
        {
            sb.AppendLine("");
            //Iterate through property collection
            foreach (var prop in propList)
            {
                //Construct property name and value string
                propStr = prop.Name + ": " + prop.GetValue(item, null);
                sb.AppendLine(propStr);
            }
        }
        return sb.ToString();
    }

    public static void ToCSV<T>(this IList<T> list, string path = "", string include = "", string exclude = "")
    {
        CreateCsvFile(list, path, include, exclude);
    }

    public static void ToExcelNoInterop<T>(this IList<T> list, string path = "", string include = "", string exclude = "")
    {
        if (path == "")
            path = Path.GetTempPath() + @"ListDataOutput.csv";
        var rtnPath = CreateCsvFile(list, path, include, exclude);

        //Open Excel from the file
        Process proc = new Process();
        //Quotes wrapped path for any space in folder/file names
        proc.StartInfo = new ProcessStartInfo("excel.exe", "\"" + rtnPath + "\"");
        proc.Start();
    }

    private static string CreateCsvFile<T>(IList<T> list, string path, string include, string exclude)
    {
        //Variables for build CSV string
        StringBuilder sb = new StringBuilder();
        List<string> propNames;
        List<string> propValues;
        bool isNameDone = false;

        //Get property collection and set selected property list
        PropertyInfo[] props = typeof(T).GetProperties();
        List<PropertyInfo> propList = GetSelectedProperties(props, include, exclude);

        //Add list name and total count
        string typeName = GetSimpleTypeName(list);
        //sb.AppendLine(string.Format("{0} List - Total Count: {1}", typeName, list.Count.ToString()));

        //Iterate through data list collection
        foreach (var item in list)
        {
            sb.AppendLine("");
            propNames = new List<string>();
            propValues = new List<string>();

            //Iterate through property collection
            foreach (var prop in propList)
            {
                //Construct property name string if not done in sb
                if (!isNameDone) propNames.Add(prop.Name);

                //Construct property value string with double quotes for issue of any comma in string type data
                var val = prop.PropertyType == typeof(string) ? "\"{0}\"" : "{0}";
                propValues.Add(string.Format(val, prop.GetValue(item, null)));
            }
            //Add line for Names
            string line = string.Empty;
            if (!isNameDone)
            {
                line = string.Join(",", propNames);
                sb.AppendLine(line);
                isNameDone = true;
            }
            //Add line for the values
            line = string.Join(",", propValues);
            sb.Append(line);
        }
        if (!string.IsNullOrEmpty(sb.ToString()) && path != "")
        {
            File.WriteAllText(path, sb.ToString());
        }
        return path;
    }
    private static List<PropertyInfo> GetSelectedProperties(PropertyInfo[] props, string include, string exclude)
    {
        List<PropertyInfo> propList = new List<PropertyInfo>();
        if (include != "") //Do include first
        {
            var includeProps = include.ToLower().Split(',').ToList();
            foreach (var item in props)
            {
                var propName = includeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                if (!string.IsNullOrEmpty(propName))
                    propList.Add(item);
            }
        }
        else if (exclude != "") //Then do exclude
        {
            var excludeProps = exclude.ToLower().Split(',');
            foreach (var item in props)
            {
                var propName = excludeProps.Where(a => a == item.Name.ToLower()).FirstOrDefault();
                if (string.IsNullOrEmpty(propName))
                    propList.Add(item);
            }
        }
        else //Default
        {
            propList.AddRange(props.ToList());
        }
        return propList;
    }

    private static string GetSimpleTypeName<T>(IList<T> list)
    {
        string typeName = list.GetType().ToString();
        int pos = typeName.IndexOf("[") + 1;
        typeName = typeName.Substring(pos, typeName.LastIndexOf("]") - pos);
        typeName = typeName.Substring(typeName.LastIndexOf(".") + 1);
        return typeName;
    }
}