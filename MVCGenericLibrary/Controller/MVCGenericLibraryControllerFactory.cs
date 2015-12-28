namespace MVCGenericLibrary.Controller
{
    using System;
    using System.Web.Mvc;
    using StructureMap;

    public class MVCGenericLibraryControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return (IController)ObjectFactory.GetInstance(controllerType);
        }
    }
}