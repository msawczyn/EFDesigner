# DatabaseInitializerKind Enumeration
 

DomainEnumeration: DatabaseInitializerKind Description for Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind

**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.3.0.0

## Syntax

**C#**<br />
``` C#
public enum DatabaseInitializerKind
```

**VB**<br />
``` VB
Public Enumeration DatabaseInitializerKind
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind.CreateDatabaseIfNotExists">**CreateDatabaseIfNotExists**</td><td>0</td><td>CreateDatabaseIfNotExists Will recreate and optionally re-seed the database only if the database does not exist.</td></tr><tr><td /><td target="F:Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind.DropCreateDatabaseAlways">**DropCreateDatabaseAlways**</td><td>1</td><td>DropCreateDatabaseAlways Will always recreate and optionally re-seed the database the first time that a context is used in the app domain.</td></tr><tr><td /><td target="F:Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind.DropCreateDatabaseIfModelChanges">**DropCreateDatabaseIfModelChanges**</td><td>2</td><td>DropCreateDatabaseIfModelChanges Will delete, recreate, and optionally re-seed the database only if the model has changed since the database was created.</td></tr><tr><td /><td target="F:Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind.MigrateDatabaseToLatestVersion">**MigrateDatabaseToLatestVersion**</td><td>3</td><td>MigrateDatabaseToLatestVersion Will use Code First Migrations to update the database to the latest version.</td></tr><tr><td /><td target="F:Sawczyn.EFDesigner.EFModel.DatabaseInitializerKind.None">**None**</td><td>4</td><td>None Null configuration. Will not check database for correctness, speeding up initialization and queries.</td></tr></table>

## See Also


#### Reference
<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />