cd EF6Parser\
msbuild /p:DeployOnBuild=true /p:PublishProfile=net472
msbuild /p:DeployOnBuild=true /p:PublishProfile=netcoreapp31

cd ..\EFCore2Parser\
msbuild /p:DeployOnBuild=true /p:PublishProfile=net472
msbuild /p:DeployOnBuild=true /p:PublishProfile=netcoreapp21
msbuild /p:DeployOnBuild=true /p:PublishProfile=netcoreapp31

cd ..\EFCore3Parser\
msbuild /p:DeployOnBuild=true /p:PublishProfile=net472
msbuild /p:DeployOnBuild=true /p:PublishProfile=netcoreapp21
msbuild /p:DeployOnBuild=true /p:PublishProfile=netcoreapp31
