using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Syntax: aver foo*.dll");
            }
            try
            {
                string fullPath = Path.Combine(Path.GetFullPath("."), args[0]);
                //string fullPath = Path.GetFullPath(args[0]);
                string path = Path.GetDirectoryName(fullPath);
                string pattern = Path.GetFileName(fullPath);
                foreach (var assemblyPath in Directory.EnumerateFiles(path, pattern))
                {
                    string ext = Path.GetExtension(assemblyPath);
                    if (ext.Equals(".dll", StringComparison.OrdinalIgnoreCase) || ext.Equals(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessOneAssembly(assemblyPath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void ProcessOneAssembly(string assemblyPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var debugAttrib = assembly.GetCustomAttribute<DebuggableAttribute>();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                Console.WriteLine("{0} ({1}, {2})",
                    assembly.FullName.ToString(),
                    fvi.FileVersion,
                    debugAttrib.IsJITTrackingEnabled ? "debug" : "retail");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: {1}", Path.GetFileName(assemblyPath), e.Message);
            }
        }
    }
}
