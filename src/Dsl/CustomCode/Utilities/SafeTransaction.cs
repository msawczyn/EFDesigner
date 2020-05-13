//using System;

//using Microsoft.VisualStudio.Modeling;

//using Sawczyn.EFDesigner.EFModel.Annotations;

//namespace Sawczyn.EFDesigner.EFModel
//{
//   public class SafeTransaction : IDisposable
//   {
//      private readonly bool isExternal;
//      [NotNull] private readonly Transaction transaction;

//      public SafeTransaction([NotNull] Store store, string name = null)
//      {
//         if (store == null)
//            throw new ArgumentNullException(nameof(store));

//         TransactionManager transactionManager = store.TransactionManager;

//         if (transactionManager.InTransaction)
//         {
//            isExternal = true;
//            transaction = transactionManager.CurrentTransaction;
//         }
//         else
//         {
//            isExternal = false;

//            transaction = name == null
//                             ? transactionManager.BeginTransaction()
//                             : transactionManager.BeginTransaction(name);
//         }
//      }

//      /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
//      public void Dispose()
//      {
//         if (!isExternal)
//            transaction.Dispose();
//      }

//      public void Commit()
//      {
//         transaction.Commit();
//      }

//      public void Rollback()
//      {
//         transaction.Rollback();
//      }

//      public bool IsSerializing
//      {
//         get { return transaction.IsSerializing; }
//      }

//      public static implicit operator Transaction(SafeTransaction st)
//      {
//         return st.transaction;
//      }
//   }
//}