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

namespace SureImpact.Data.Framework
{
   public partial class Entity1
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected Entity1()
      {
         TestDatas_Entity2 = new System.Collections.Generic.HashSet<global::SureImpact.Data.Framework.Entity2>();
         _testDatas = new System.Collections.Generic.HashSet<global::SureImpact.Data.Framework.TestData>();

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
      /// <param name="teststring">Test string</param>
      public Entity1(string teststring)
      {
         if (string.IsNullOrEmpty(teststring)) throw new ArgumentNullException(nameof(teststring));
         this.TestString = teststring;

         TestDatas_Entity2 = new System.Collections.Generic.HashSet<global::SureImpact.Data.Framework.Entity2>();
         _testDatas = new System.Collections.Generic.HashSet<global::SureImpact.Data.Framework.TestData>();
         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="teststring">Test string</param>
      public static Entity1 Create(string teststring)
      {
         return new Entity1(teststring);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// Unique identifier
      /// </summary>
      [Key]
      [Required]
      [System.ComponentModel.Description("Unique identifier")]
      public long Id { get; set; }

      /// <summary>
      /// Indexed, Required, Max length = 200
      /// Test string
      /// </summary>
      [Required]
      [MaxLength(200)]
      [StringLength(200)]
      [System.ComponentModel.Description("Test string")]
      public string TestString { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

      /// <summary>
      /// Backing field for TestDatas
      /// </summary>
      protected ICollection<global::SureImpact.Data.Framework.TestData> _testDatas;

      public virtual ICollection<global::SureImpact.Data.Framework.TestData> TestDatas
      {
         get
         {
            return _testDatas;
         }
         private set
         {
            _testDatas = value;
         }
      }

      /// <summary>
      /// Association class for TestDatas
      /// </summary>
      [System.ComponentModel.Description("Association class for TestDatas")]
      [System.ComponentModel.DataAnnotations.Display(Name="Association object for TestDatas")]
      public virtual ICollection<global::SureImpact.Data.Framework.Entity2> TestDatas_Entity2 { get; private set; }

   }
}

