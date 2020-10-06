using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox_EF6_Test
{
   class Program
   {
      static void Main(string[] args)
      {
         Console.WriteLine("The players -");
         Console.WriteLine("===================================");
         Console.WriteLine("public partial class Entity1");
         Console.WriteLine("{");
         Console.WriteLine("   public Entity1()");
         Console.WriteLine("   {");
         Console.WriteLine("      Entity2 = new HashSet<Entity2>();");
         Console.WriteLine("   }");
         Console.WriteLine("");
         Console.WriteLine("   public int Id { get; protected set; }");
         Console.WriteLine("   public virtual ICollection<Entity2> Entity2 { get; protected set; }");
         Console.WriteLine("}");
         Console.WriteLine("");
         Console.WriteLine("public partial class Entity2");
         Console.WriteLine("{");
         Console.WriteLine("   public Entity2()");
         Console.WriteLine("   {");
         Console.WriteLine("   }");
         Console.WriteLine("");
         Console.WriteLine("   public int Id { get; protected set; }");
         Console.WriteLine("   public int? FK { get; set; }");
         Console.WriteLine("   public virtual Entity1 { get; set; }");
         Console.WriteLine("}");
         Console.WriteLine("");

         using (Context context = new Context())
         {
            Entity1 e1a = context.Entity1.Add(new Entity1());
            Entity1 e1b = context.Entity1.Add(new Entity1());
            Entity1 e1c = context.Entity1.Add(new Entity1());
            Entity2 e2 = context.Entity2.Add(new Entity2());
            context.SaveChanges();

            int e1aId = e1a.Id;
            int e1bId = e1b.Id;
            int e1cId = e1c.Id;
            int e2Id = e2.Id;

            Console.WriteLine("Initial values:");
            Console.WriteLine("===================================");
            Console.WriteLine($"   e1a.Id = {e1aId}");
            Console.WriteLine($"   e1b.Id = {e1bId}");
            Console.WriteLine($"   e1c.Id = {e1cId}");
            Console.WriteLine($"   e2.Id  = {e2Id}");
            Console.WriteLine();

            Console.WriteLine("===================================");
            Console.WriteLine("Set the navigation property, but not the foreign key");
            Console.WriteLine("===================================");
            Console.WriteLine();

            Console.WriteLine("e2.Entity1 = e1a;");
            e2.Entity1 = e1a;
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("SaveChanges");
            context.SaveChanges();
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("===================================");
            Console.WriteLine("Set the foreign key, but not the navigation property");
            Console.WriteLine("===================================");
            Console.WriteLine($"e2.FK = {e1bId};");
            e2.FK = e1bId;
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("SaveChanges");
            context.SaveChanges();
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("===================================");
            Console.WriteLine("Set the foreign key to the same value and the navigation property to a different value");
            Console.WriteLine("===================================");
            Console.WriteLine($"e2.Entity1 = e1c;");
            Console.WriteLine($"e2.FK = {e1bId};");
            e2.Entity1 = e1c;
            e2.FK = e1bId;
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("SaveChanges");
            context.SaveChanges();
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            Console.WriteLine("===================================");
            Console.WriteLine("Set the foreign key and the navigation property to different values");
            Console.WriteLine("===================================");
            Console.WriteLine($"e2.Entity1 = e1a;");
            Console.WriteLine($"e2.FK = {e1bId};");
            e2.Entity1 = e1a;
            e2.FK = e1bId;
            Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
            Console.WriteLine($"   e2.FK = {e2.FK}");
            Console.WriteLine();

            try
            {
               Console.WriteLine("SaveChanges");
               context.SaveChanges();
               Console.WriteLine($"   e2.Entity1.Id = {e2.Entity1.Id}");
               Console.WriteLine($"   e2.FK = {e2.FK}");
            }
            catch (Exception e)
            {
               Console.WriteLine($"   {e.GetType().Name}: {e.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("Hit return to continue");
            Console.ReadLine();
         }
      }
   }
}
