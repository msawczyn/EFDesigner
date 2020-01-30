using System.Diagnostics.CodeAnalysis;

namespace ParsingModels
{
   public class ModelUnidirectionalAssociation
   {
      public string SourceClassName { get; set; }
      public string SourceClassNamespace { get; set; }
      public string SourceClassFullName => string.IsNullOrWhiteSpace(SourceClassNamespace) ? $"global::{SourceClassName}" : $"global::{SourceClassNamespace}.{SourceClassName}";
      public Multiplicity SourceMultiplicity { get; set; }
      public AssociationRole SourceRole { get; set; } = AssociationRole.NotSet;

      public string TargetClassName { get; set; }
      public string TargetClassNamespace { get; set; }
      public string TargetClassFullName => string.IsNullOrWhiteSpace(TargetClassNamespace) ? $"global::{TargetClassName}" : $"global::{TargetClassNamespace}.{TargetClassName}";
      public Multiplicity TargetMultiplicity { get; set; }
      public string TargetPropertyTypeName { get; set; }
      public string TargetPropertyName { get; set; }
      public string TargetSummary { get; set; }
      public string TargetDescription { get; set; }
      public string ForeignKey { get; set; }
      public AssociationRole TargetRole { get; set; } = AssociationRole.NotSet;

      /// <summary>Determines whether the specified object is equal to the current object.</summary>
      /// <param name="obj">The object to compare with the current object.</param>
      /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
      [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
      public override bool Equals(object obj) {
         if (ReferenceEquals(null, obj))
            return false;

         if (ReferenceEquals(this, obj))
            return true;

         if (obj.GetType() != this.GetType())
            return false;

         return Equals((ModelUnidirectionalAssociation)obj);
      }

      protected bool Equals(ModelUnidirectionalAssociation other)
      {
         return SourceClassName == other.SourceClassName
             && SourceClassNamespace == other.SourceClassNamespace
             && SourceMultiplicity == other.SourceMultiplicity
             && SourceRole == other.SourceRole
             && TargetClassName == other.TargetClassName
             && TargetClassNamespace == other.TargetClassNamespace
             && TargetMultiplicity == other.TargetMultiplicity
             && TargetPropertyTypeName == other.TargetPropertyTypeName
             && TargetPropertyName == other.TargetPropertyName
             && TargetRole == other.TargetRole;
      }

      /// <summary>Serves as the default hash function.</summary>
      /// <returns>A hash code for the current object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hashCode = SourceClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ SourceClassNamespace.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceMultiplicity;
            hashCode = (hashCode * 397) ^ (int)SourceRole;
            hashCode = (hashCode * 397) ^ TargetClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ TargetClassNamespace.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)TargetMultiplicity;
            hashCode = (hashCode * 397) ^ TargetPropertyTypeName.GetHashCode();
            hashCode = (hashCode * 397) ^ TargetPropertyName.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)TargetRole;

            return hashCode;
         }
      }

      /// <summary>Returns a value that indicates whether the values of two <see cref="T:ParsingModels.ModelUnidirectionalAssociation" /> objects are equal.</summary>
      /// <param name="left">The first value to compare.</param>
      /// <param name="right">The second value to compare.</param>
      /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
      public static bool operator ==(ModelUnidirectionalAssociation left, ModelUnidirectionalAssociation right) { return Equals(left, right); }

      /// <summary>Returns a value that indicates whether two <see cref="T:ParsingModels.ModelUnidirectionalAssociation" /> objects have different values.</summary>
      /// <param name="left">The first value to compare.</param>
      /// <param name="right">The second value to compare.</param>
      /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
      public static bool operator !=(ModelUnidirectionalAssociation left, ModelUnidirectionalAssociation right) { return !Equals(left, right); }
   }
}