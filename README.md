# Reproduction of Source Generator File Locking Issue

When a project references a Source Generator, changes to the project while using `dotnet watch` can cause warning
MSB3026, error MSB3027, and error MSB3021. This is due to the source generator project's output DLL being locked.

<details>
<summary>Example output</summary>
<pre>
dotnet watch ⌚ File changed: .\MyExample.WidgetApi\MyExample.WidgetApi.csproj.
dotnet watch ⌚ Exited
dotnet watch 🔧 Building...
  MyExample.ThisIsASourceGenerator failed with 2 error(s) and 10 warning(s) (12.8s)
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 1 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 2 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 3 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 4 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 5 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 6 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 7 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 8 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 9 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): warning MSB3026: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.This
IsASourceGenerator.dll". Beginning retry 10 in 1000ms. The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): error MSB3027: Could not copy "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll". Exceeded retry count of 10. Failed. The file is locked by: ".NET Host (20364)"
    C:\Program Files\dotnet\sdk\8.0.300\Microsoft.Common.CurrentVersion.targets(4806,5): error MSB3021: Unable to copy file "obj\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll" to "bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll". The process cannot access the file 'I:\code\lockrepro\MyExample.ThisIsASourceGenerator\bin\Any CPU\Debug\net8.0\MyExample.ThisIsASourceGenerator.dll' because it is being used by another process.      

Build failed with 2 error(s) and 10 warning(s) in 14.1s
</pre>
</details>

## Repository Details
This repository contains four projects.
 - `MyExample.Data` contains an Entity Framework Core data context
 - `MyExample.ThisIsASourceGenerator` is a source generator which adds methods to configure the EF Core data context
    from `MyExample.Data`
 - `MyExample.WidgetApi` is an ASP.NET Web API project which uses the EF Core data context
 - `MyExample.SprocketApi` is an ASP.NET Web API project which does _not_ reference these other projects

## Reproduction Steps
As a convenience, `run.ps1` in the root of the repository starts `dotnet watch` for both Web API projects. A variety of
changes to the `MyExample.WidgetApi` will cause the above output due to file locking, whereas similar changes to
`MyExample.SprocketApi` will _not_ cause the issue. In a real world project, these issues have been noticed with a wide
variety of code changes (which may or may not be classified as "rude"). However, the simplest reproduction is to change
the version of the references NuGet packages. The `reproduce.ps1` script will start `dotnet watch` for both projects and
then cycle through commits in this repository in order to attempt to recreate the error. 
