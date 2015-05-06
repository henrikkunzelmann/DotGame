using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotGame.EntitySystem;
using System.Numerics;
using System.Diagnostics;

namespace DotGame.Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var scene = new Scene();

            var entity = scene.CreateChild("Test 1");
            entity.Transform.LocalPosition = new Vector3(4, 12, 32);
            var camera = entity.AddComponent<EntitySystem.Components.Camera>();


            var entity2 = scene.CreateChild("Test 2");
            entity2.Transform.LocalPosition = new Vector3(4, 12, 32);
            entity2.Transform.Parent = entity.Transform;

            var amt = 1000;
            Console.BackgroundColor = ConsoleColor.DarkBlue; TimerUtils.Start("Creating Prefab");
            Prefab prefab = null;
            for (int i = 0; i < amt; i++)
                prefab = Prefab.FromEntity(entity);
            Console.WriteLine(TimerUtils.DefaultString("Creating Prefab", amt, true));

            Console.BackgroundColor = ConsoleColor.DarkYellow; TimerUtils.Start("Creating Instance (1st time)");
            Entity instance = prefab.CreateInstance(new Scene());
            Console.WriteLine(TimerUtils.DefaultString("Creating Instance (1st time)", 1, true));

            Console.BackgroundColor = ConsoleColor.DarkCyan; TimerUtils.Start("Creating Instance");
            for (int i = 0; i < amt; i++)
                instance = prefab.CreateInstance(new Scene());
            Console.WriteLine(TimerUtils.DefaultString("Creating Instance", amt, true));

            Console.BackgroundColor = ConsoleColor.DarkGreen; TimerUtils.Start("Setting Parent");
            instance.Transform.Parent = entity.Transform;
            Console.WriteLine(TimerUtils.DefaultString("Setting Parent", 1, true));

            Console.BackgroundColor = ConsoleColor.DarkMagenta; TimerUtils.Start("Invoking Event (1st time)");
            entity.Invoke("Update", false, new GameTime());
            Console.WriteLine(TimerUtils.DefaultString("Invoking Event (1st time)", 1, true));

            Console.BackgroundColor = ConsoleColor.DarkRed; TimerUtils.Start("Invoking Event");
            for (int i = 0; i < amt; i++)
                entity.Invoke("Update", false, new GameTime());
            Console.WriteLine(TimerUtils.DefaultString("Invoking Event", amt, true));

            Console.ReadKey();
        }
    }
}
