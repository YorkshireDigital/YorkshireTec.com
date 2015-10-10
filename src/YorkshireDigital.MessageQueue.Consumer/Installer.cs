using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;
using YorkshireDigital.MessageQueue.Consumer;

public class Installer : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        container.Register(
            Component.For<IConsumerService>().ImplementedBy<ConsumerService>().LifestyleTransient(),
            Component.For<IBus>().UsingFactoryMethod(BusBuilder.CreateMessageBus).LifestyleSingleton());
    }
}