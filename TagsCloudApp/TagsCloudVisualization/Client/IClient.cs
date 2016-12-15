using Castle.Windsor;

namespace TagsCloudVisualization.Client
{
    interface IClient
    {
        void Run(IWindsorContainer container, Options options);
    }
}
