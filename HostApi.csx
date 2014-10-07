#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Api\bin\Microsoft.Owin.Hosting.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\\YorkshireDigital.Api\\bin\\YorkshireDigital.Api.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.Host.HttpListener.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.FileSystems.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.Hosting.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Microsoft.Owin.StaticFiles.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\Owin.dll"
#r "D:\Code\GitHub\YorkshireTec.com\src\YorkshireDigital.Web\bin\YorkshireDigital.Web.dll"

using System;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Owin.Hosting;
using YorkshireDigital.Api.Infrastructure;

    // get the name of the assembly
    var exeAssembly = typeof(YorkshireDigital.Api.Startup).Assembly.FullName;
    var webconfig = typeof(YorkshireDigital.Api.Startup).Assembly.Location + ".config";
    Console.WriteLine(webconfig);
    // setup - there you put the path to the config file
    var setup = new AppDomainSetup
    {
        ApplicationBase = Environment.CurrentDirectory,
        ConfigurationFile = webconfig
    };

    // create the app domain
    var appDomain = AppDomain.CreateDomain("Web AppDomain", null, setup);

    // call the startup method - something like alternative main()
    WebApp.Start<YorkshireDigital.Api.Startup>("http://+:61140");

    // in the end, unload the domain
    AppDomain.Unload(appDomain);
