using Autofac;

namespace Hanlin.Program
{
    public abstract class ProgramBase
    {
        protected IContainer Container { get; set; }

        protected void BuildContainer()
        {
            var autofac = new ContainerBuilder();
            ConfigContainer(autofac);
            Container = autofac.Build();
        }

        protected abstract void ConfigContainer(ContainerBuilder autofac);

        protected virtual void Run()
        {
            Container.Resolve<IRunnable>().Run();
        }
    }
}