using System;
using System.IO;
using System.Linq;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace build
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Target("build", () =>
            {
                var files = Directory.GetFiles(".", "*.sln", SearchOption.AllDirectories)
                    .Where(f => !f.Contains("webform", StringComparison.OrdinalIgnoreCase));

                foreach (var file in files)
                {
                    Run("dotnet", $"build {file} -c Release --nologo");
                }
            });

            Target("default", DependsOn("build"));

            RunTargetsAndExit(args);
        }
    }
}