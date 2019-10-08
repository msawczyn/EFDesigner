## Finding tracking properties

Search for `new TrackingPropertyDescriptor`

## Adding a tracking property

#### Assumptions
- Master property (FooPrime) already exists in source entity
- We want to add a tracking property (Foo) to some target entity that tracks Source.FooPrime

#### Steps
 1. Add Foo to target entity
    a. Set `Kind` to `CustomStorage`
    b. Set `IsBrowsable` to `False`
 2. Add `bool IsFooTracking` to target entity
    a. Set `Default Value` to `true`
    b. Set `IsBrowsable` to `False`
 3. Transform templates
 4. Open (or add) the partial for the target's TypeDescriptor
    a. Since Foo is not browsable, add a property descriptor for Foo following the existing pattern
```
      DomainPropertyInfo fooPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Target.FooDomainPropertyId);
      DomainPropertyInfo isFooTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Target.IsFooTrackingDomainPropertyId);

      // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
      Attribute[] fooAttributes =
      {
         new DisplayNameAttribute("Foo"),
         new DescriptionAttribute("Foo description"),
         new CategoryAttribute("Foo category")
      };

      propertyDescriptors.Add(new TrackingPropertyDescriptor(target, fooPropertyInfo, isFooTrackingPropertyInfo, fooAttributes));
```
Or, more succinctly:
```
      propertyDescriptors.Add(new TrackingPropertyDescriptor(target, 
                                                             storeDomainDataDirectory.GetDomainProperty(Target.FooDomainPropertyId), 
                                                             storeDomainDataDirectory.GetDomainProperty(Target.IsFooTrackingDomainPropertyId), 
                                                             new Attribute[] { new DisplayNameAttribute("Foo"),
                                                                               new DescriptionAttribute("Foo description"),
                                                                               new CategoryAttribute("Foo category")
                                                                             }));
```
 5. If the TypeDescriptor was newly added, register it in the custom partial for the package at DslPackage/CustomCode/Partials/EFModelPackage.cs
 6. In the partial for Source, add a value changed handler for FooPrime
```
      #region FooPrime tracking property

      protected virtual void OnFooPrimeChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, <appropriate collection>, Target.FooDomainPropertyId, Target.IsFooTrackingDomainPropertyId);
      }

      internal sealed partial class FooPrimePropertyHandler
      {
         protected override void OnValueChanged(Source element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnFooPrimeChanged(oldValue, newValue);
         }
      }

      #endregion
```
 7. Add storage, getter and setter for Target.Foo in the partial for Target
```
      private FooType fooStorage;

      public FooType GetFooValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsFooTracking)
               try
               {
                  return Source?.FooMaster;
               }
               catch (NullReferenceException)
               {
                  return default(FooType);
               }
               catch (Exception e)
               {
                  if (CriticalException.IsCriticalException(e))
                     throw;

                  return default(FooType);
               }

         return fooStorage;
      }

      public void SetFooValue(FooType value)
      {
         fooStorage = (value == Source.FooMaster) ? default(FooType) : value;
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsFooTracking = (fooStorage == default(FooType)); // or whatever the value is for the tracked property
      }
```
 8. Also in the partial for Target, add a PropertyHandler for the Target.Foo
```
      internal sealed partial class IsFooTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsFooTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(Target element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(FooDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsFooTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(Target element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = Source.FooMaster;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != default(FooType) && element.Foo == (FooType)calculatedValue)
               element.isFooTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsFooTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(Target element)
         {
            // Force the IsFooTracking property to false so that the value  
            // of the Foo property is retrieved from storage.  
            element.isFooTrackingPropertyStorage = false;
         }
      }
```
 9. Add/Update the ResetIsTrackingProperties and PreResetIsTrackingProperties methods in the partial for Target following the pattern of the existing code.
10. If Target has never had a tracking property before, update `EFModelSerializationHelperBase.ResetTrackingProperties` 
in `Dsl/Custom Code/Partials/EFModelSerializationHelperBase.cs` following the pattern of the existing code.

#### Summary

**Modify the following files:**

1. **DSL model**
   a. Add target property _(step 1)_
   b. Add IsXXXTracking property _(step 2)_
2. **Target entity type descriptor (in DSL/Custom Code/Type Descriptors)**
   a. Add type descriptor for target property _(step 4)_
   b. If type descriptor is a new file, update `EFModelPackage.cs` _(step 5)_
3. **Source partial**
   a. Add value changed handler _(step 6)_
4. **Target partial**
   a. Add property storage, getter and setter for new property _(step 7)_
   b. Add property handler class for IsXXXTracking property _(step 8)_
   c. Add/Update the ResetIsTrackingProperties and PreResetIsTrackingProperties methods _(step 9)_
   d. If target has never had a tracking property before, update EFModelSerializationHelperBase _(step 10)_

