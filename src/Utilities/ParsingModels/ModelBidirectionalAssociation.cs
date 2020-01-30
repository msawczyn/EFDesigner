using System.Diagnostics.CodeAnalysis;

namespace ParsingModels
{
   public class ModelBidirectionalAssociation : ModelUnidirectionalAssociation
   {
      public string SourcePropertyTypeName { get; set; }
      public string SourcePropertyName { get; set; }
      public string SourceSummary { get; set; }
      public string SourceDescription { get; set; }

      public ModelBidirectionalAssociation Inverse()
      {
         return
            new ModelBidirectionalAssociation
            {
               SourceClassName = TargetClassName
             , SourceClassNamespace = TargetClassNamespace
             , SourceMultiplicity = TargetMultiplicity
             , SourcePropertyTypeName = TargetPropertyTypeName
             , SourcePropertyName = TargetPropertyName
             , TargetClassName = SourceClassName
             , TargetClassNamespace = SourceClassNamespace
             , TargetMultiplicity = SourceMultiplicity
             , TargetPropertyTypeName = SourcePropertyTypeName
             , TargetPropertyName = SourcePropertyName
            };
      }

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

         return Equals((ModelBidirectionalAssociation)obj);
      }

      protected bool Equals(ModelBidirectionalAssociation other)
      {
         return (base.Equals(other)
              && SourcePropertyTypeName == other.SourcePropertyTypeName
              && SourcePropertyName == other.SourcePropertyName);
      }

      /// <summary>Serves as the default hash function.</summary>
      /// <returns>A hash code for the current object.</returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ SourcePropertyTypeName.GetHashCode();
            hashCode = (hashCode * 397) ^ SourcePropertyName.GetHashCode();

            return hashCode;
         }
      }

      /// <summary>Returns a value that indicates whether the values of two <see cref="T:ParsingModels.ModelBidirectionalAssociation" /> objects are equal.</summary>
      /// <param name="left">The first value to compare.</param>
      /// <param name="right">The second value to compare.</param>
      /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
      public static bool operator ==(ModelBidirectionalAssociation left, ModelBidirectionalAssociation right) { return Equals(left, right); }

      /// <summary>Returns a value that indicates whether two <see cref="T:ParsingModels.ModelBidirectionalAssociation" /> objects have different values.</summary>
      /// <param name="left">The first value to compare.</param>
      /// <param name="right">The second value to compare.</param>
      /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
      public static bool operator !=(ModelBidirectionalAssociation left, ModelBidirectionalAssociation right) { return !Equals(left, right); }
   }
}