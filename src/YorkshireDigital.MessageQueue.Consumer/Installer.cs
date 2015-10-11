using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;

namespace YorkshireDigital.MessageQueue.Consumer
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IConsumerService>().ImplementedBy<ConsumerService>().LifestyleTransient(),
                Component.For<IBus>().UsingFactoryMethod(_ => BusBuilder.CreateMessageBus()).LifestyleSingleton());
        }
    }
}