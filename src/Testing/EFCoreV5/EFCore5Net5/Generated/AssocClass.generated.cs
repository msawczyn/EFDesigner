//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v4.1.2.0
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
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
   public partial class AssocClass
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected AssocClass()
      {
         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static AssocClass CreateAssocClassUnsafe()
      {
         return new AssocClass();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="id">Unique identifier</param>
      /// <param name="entity2id">Foreign key for EntityImplementation.Entity2_Entity3 &lt;--&gt; Entity3.EntityImplementations.</param>
      /// <param name="entityimplementationsid">Foreign key for Entity2.EntityImplementations_Entity3 &lt;--&gt; Entity3.Entity2. </param>
      /// <param name="entityimplementations">Association class for EntityImplementations</param>
      /// <param name="entity2">Association class for Entity2</param>
      public AssocClass(long id, long entity2id, long entityimplementationsid, global::Testing.EntityImplementation entityimplementations, global::Testing.Entity2 entity2)
      {
         this.Id = id;

         this.Entity2Id = entity2id;

         this.EntityImplementationsId = entityimplementationsid;

         if (entityimplementations == null) throw new ArgumentNullException(nameof(entityimplementations));
         this.EntityImplementations = entityimplementations;
         entityimplementations.Entity2_Entity3.Add(this);

         if (entity2 == null) throw new ArgumentNullException(nameof(entity2));
         this.Entity2 = entity2;
         entity2.EntityImplementations_Entity3.Add(this);

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="id">Unique identifier</param>
      /// <param name="entity2id">Foreign key for EntityImplementation.Entity2_Entity3 &lt;--&gt; Entity3.EntityImplementations.</param>
      /// <param name="entityimplementationsid">Foreign key for Entity2.EntityImplementations_Entity3 &lt;--&gt; Entity3.Entity2. </param>
      /// <param name="entityimplementations">Association class for EntityImplementations</param>
      /// <param name="entity2">Association class for Entity2</param>
      public static AssocClass Create(long id, long entity2id, long entityimplementationsid, global::Testing.EntityImplementation entityimplementations, global::Testing.Entity2 entity2)
      {
         return new AssocClass(id, entity2id, entityimplementationsid, entityimplementations, entity2);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// Foreign key for EntityImplementation.Entity2_Entity3 &lt;--&gt; Entity3.EntityImplementations.
      /// </summary>
      [Key]
      [Required]
      [System.ComponentModel.Description("Foreign key for EntityImplementation.Entity2_Entity3 <--> Entity3.EntityImplementations. ")]
      public long Entity2Id { get; set; }

      /// <summary>
      /// Identity, Indexed, Required
      /// Foreign key for Entity2.EntityImplementations_Entity3 &lt;--&gt; Entity3.Entity2. 
      /// </summary>
      [Key]
      [Required]
      [System.ComponentModel.Description("Foreign key for Entity2.EntityImplementations_Entity3 <--> Entity3.Entity2. ")]
      public long EntityImplementationsId { get; set; }

      /// <summary>
      /// Indexed, Required
      /// Unique identifier
      /// </summary>
      [Required]
      [System.ComponentModel.Description("Unique identifier")]
      public long Id { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Required&lt;br/&gt;
      /// Association class for Entity2
      /// </summary>
      [System.ComponentModel.Description("Association class for Entity2")]
      [System.ComponentModel.DataAnnotations.Display(Name="Association object for Entity2")]
      public virtual global::Testing.Entity2 Entity2 { get; set; }

      /// <summary>
      /// Required&lt;br/&gt;
      /// Association class for EntityImplementations
      /// </summary>
      [System.ComponentModel.Description("Association class for EntityImplementations")]
      [System.ComponentModel.DataAnnotations.Display(Name="Association object for EntityImplementations")]
      public virtual global::Testing.EntityImplementation EntityImplementations { get; set; }

   }
}
