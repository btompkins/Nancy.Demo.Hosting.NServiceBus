Nancy.Demo.Hosting.NServiceBus
==============================
Simple demo to show strange behavior when self-hosting NancyFx within the NServiceBus host.

This demo will currently NOT work when hosted as a service, since the call to Nancy's FileSystemRootPathProvider 

https://github.com/NancyFx/Nancy/blob/master/src/Nancy.Hosting.Self/FileSystemRootPathProvider.cs

Makes a call to Assembly.GetEntryAssembly() which will return NULL when the NServiceBus host is running as a service, 
after which Nancy will look for views etc in System32.

I'm not sure WHY this is happening, as one friend said that's some deep voodoo there. :)  But one fix is to 
use Assembly.GetExecutingAssembly() instead of Environment.CurrentDirectory So this: 

  return assembly == null ? Environment.CurrentDirectory :
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)

Becomes 

 return assembly == null ?  Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) :
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
