//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor
//     https://github.com/msawczyn/EFDesigner
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Testing
{
   public partial class Entity1
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Entity1()
      {
         Entity11 = new System.Collections.Generic.HashSet<global::Testing.Entity11>();
         Entity2_1 = new System.Collections.Generic.HashSet<global::Testing.Entity2>();

         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static Entity1 CreateEntity1Unsafe()
      {
         return new Entity1();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="entity2"></param>
      public Entity1(global::Testing.Entity2 entity2)
      {
         if (entity2 == null) throw new ArgumentNullException(nameof(entity2));
         this.Entity2 = entity2;

         this.Entity11 = new System.Collections.Generic.HashSet<global::Testing.Entity11>();
         this.Entity2_1 = new System.Collections.Generic.HashSet<global::Testing.Entity2>();

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="entity2"></param>
      public static Entity1 Create(global::Testing.Entity2 entity2)
      {
         return new Entity1(entity2);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// </summary>
      [Key]
      [Required]
      public long Id { get; protected set; }

      /// <summary>
      /// Max length = 25
      /// </summary>
      [MaxLength(25)]
      [StringLength(25)]
      public string Property1 { get; set; }

      /// <summary>
      /// Max length = 25
      /// </summary>
      [MaxLength(25)]
      [StringLength(25)]
      public string Property2 { get; set; }

      /// <summary>
      /// Max length = 25
      /// </summary>
      [MaxLength(25)]
      [StringLength(25)]
      public string Property3 { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required
      /// </summary>
      public virtual global::Testing.Entity2 Entity2 { get; set; }

      public virtual ICollection<global::Testing.Entity11> Entity11 { get; protected set; }

      public virtual ICollection<global::Testing.Entity2> Entity2_1 { get; protected set; }

   }
}
