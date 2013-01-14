Nancy.Demo.Hosting.NServiceBus
==============================
Simple demo to show strange behavior when self-hosting NancyFx within the NServiceBus host.

This demo will currently NOT work when hosted as a service, since the call to Nancy's FileSystemRootPathProvider 

https://github.com/NancyFx/Nancy/blob/master/src/Nancy.Hosting.Self/FileSystemRootPathProvider.cs

Makes a call to Assembly.GetEntryAssembly() which will return NULL when the NServiceBus host is running as a service, 
after which Nancy will look for views etc in System32.

Nancy.RequestExecutionException: Oh noes! ---> Nancy.ViewEngines.ViewNotFoundException: Unable to locate view 'staticview'
Currently available view engine extensions: sshtml,html,htm
Locations inspected: ,,,,,,,,views/Test/staticview-en-US,views/Test/staticview,Test/staticview-en-US,Test/staticview,views/staticview-en-US,views/staticview,staticview-en-US,staticview
Root path: C:\Windows\system32
   at Nancy.ViewEngines.DefaultViewFactory.GetRenderedView(String viewName, Object model, ViewLocationContext viewLocationContext)
   at System.Dynamic.UpdateDelegates.UpdateAndExecute4[T0,T1,T2,T3,TRet](CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
   at Nancy.ViewEngines.DefaultViewFactory.RenderView(String viewName, Object model, ViewLocationContext viewLocationContext)
   at System.Dynamic.UpdateDelegates.UpdateAndExecute4[T0,T1,T2,T3,TRet](CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
   at Nancy.Responses.Negotiation.ViewProcessor.Process(MediaRange requestedMediaRange, Object model, NancyContext context)
   at System.Dynamic.UpdateDelegates.UpdateAndExecute4[T0,T1,T2,T3,TRet](CallSite site, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
   at Nancy.Routing.DefaultRouteInvoker.NegotiateResponse(IEnumerable`1 compatibleHeaders, Object model, Negotiator negotiator, NancyContext context)
   at Nancy.Routing.DefaultRouteInvoker.ProcessAsNegotiator(Object routeResult, NancyContext context)
   at System.Dynamic.UpdateDelegates.UpdateAndExecute3[T0,T1,T2,TRet](CallSite site, T0 arg0, T1 arg1, T2 arg2)
   at Nancy.Routing.DefaultRouteInvoker.InvokeRouteWithStrategy(Object result, NancyContext context)
   at System.Dynamic.UpdateDelegates.UpdateAndExecute3[T0,T1,T2,TRet](CallSite site, T0 arg0, T1 arg1, T2 arg2)
   at Nancy.Routing.DefaultRouteInvoker.Invoke(Route route, DynamicDictionary parameters, NancyContext context)
   at Nancy.Routing.DefaultRequestDispatcher.Dispatch(NancyContext context)
   at Nancy.NancyEngine.InvokeRequestLifeCycle(NancyContext context, IPipelines pipelines)
   --- End of inner exception stack trace ---
   at Nancy.NancyEngine.InvokeOnErrorHook(NancyContext context, ErrorPipeline pipeline, Exception ex)

I'm not sure WHY this is happening, as one friend said that's some deep voodoo there. :)  But one fix is to 
use Assembly.GetExecutingAssembly() instead of Environment.CurrentDirectory So this: 

  return assembly == null ? Environment.CurrentDirectory :
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)

Becomes 

 return assembly == null ?  Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) :
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
