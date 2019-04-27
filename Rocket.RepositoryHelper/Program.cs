using Rocket.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rocket.RepositoryHelper
{
    public class CommandList
    {
        public List<CommandListEntry> Commands { get; set; } = new List<CommandListEntry>();
    }

    public class CommandListEntry
    {
        public CommandListEntry() { }
        public CommandListEntry(IRocketCommand command)
        {
            this.Aliases = command.Aliases;
            this.AllowedCaller = command.AllowedCaller;
            this.Help = command.Help;
            this.Name = command.Name;
            this.Permissions = command.Permissions;
            this.Syntax = command.Syntax;
        }

        public List<string> Aliases { get; set; }
        public AllowedCaller AllowedCaller { get; set; }
        public string Help { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
        public string Syntax { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            string file =  args[0];
            string dll = file;
            if (file.EndsWith(".csproj"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                dll = doc.GetElementsByTagName("AssemblyName")[0].FirstChild.Value+".dll";
            }
            handlePlugin(dll);
        }

        public static void handlePlugin(string dll)
        {
            Assembly a = Assembly.Load(File.ReadAllBytes(dll));
            string name = a.GetName().Name;
            Type[] types;
            List<Type> commands = new List<Type>();
            try
            {
                types = a.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }

            foreach (var t in types.Where(t => t != null))
            {
                if (t.IsAbstract || !t.IsClass) continue;
                if (t.GetInterface("IRocketCommand") != null)
                {
                    commands.Add(t);
                }
                if (t.BaseType == typeof(Rocket.Core.Plugins.RocketPlugin))
                {
                    getTranslation(name, t);
                }
                else if (t.BaseType.Name == "RocketPlugin`1")
                {
                    getTranslation(name, t);
                    getConfiguration(name, t);
                }
            }
            getCommands(name, commands);
        }

        public static void getCommands(string name, List<Type> types)
        {
            try
            {
                CommandList c = new CommandList();
                foreach(Type t in types)
                {
                    try
                    {
                        c.Commands.Add(new CommandListEntry((IRocketCommand)FormatterServices.GetSafeUninitializedObject(t)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Failed to serialize command "+t.FullName);
                    }
                }
                serializeWrite("commands.xml", typeof(CommandList), c);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void getTranslation(string name, Type t)
        {
            try
            {
                var plugin = FormatterServices.GetSafeUninitializedObject(t);
                serializeWrite("translation.xml", typeof(API.Collections.TranslationList), ((IRocketPlugin)plugin).DefaultTranslations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void getConfiguration(string name,Type t)
        {
            try
            {
                Type c = t.BaseType.GetGenericArguments().FirstOrDefault();
                object config = Activator.CreateInstance(c);
                if (c.GetInterface("IRocketPluginConfiguration") != null)
                {
                    ((IRocketPluginConfiguration)config).LoadDefaults();
                }
                serializeWrite("configuration.xml", c, config);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void serializeWrite(string path,Type t,object o)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                XmlSerializer s = new XmlSerializer(t);
                s.Serialize(writer, o);
            }     
        }
    }
}
