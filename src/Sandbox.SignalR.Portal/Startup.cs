using Microsoft.Owin;

using Owin;

using Sandbox.SignalR.Portal;

[assembly: OwinStartup(typeof (Startup))]

namespace Sandbox.SignalR.Portal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}